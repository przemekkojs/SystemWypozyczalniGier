using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemWypozyczalniGier.Database;
using SystemWypozyczalniGier.Helpers;
using SystemWypozyczalniGier.Tables;

namespace SystemWypozyczalniGier.Controllers
{
    public class CartController : Controller
    {
        private readonly DatabaseContext _context;

        public CartController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cart = CartHelper.GetCart(Request);
            var dbGames = _context.Games
                .Where(g => cart.Contains(g.GameId))
                .Include(g => g.Publisher)
                .Include(g => g.Reviews);

            var games = await dbGames.ToListAsync();
            ViewBag.TotalPrice = CalculateCartTotalPrice(games);
            (ViewBag.PromotionedGame, ViewBag.PromotionedGamePrice) = games.Count >= 3 ?
                GetChepestGameAndItsPromotionPrice(games) : (null, 0);

            return View(games);
        }

        public async Task<IActionResult> Payment()
        {
            var cart = CartHelper.GetCart(Request);
            var dbGames = _context.Games
                .Where(g => cart.Contains(g.GameId))
                .Include(g => g.Publisher)
                .Include(g => g.Reviews);

            var games = await dbGames.ToListAsync();
            ViewBag.TotalPrice = CalculateCartTotalPrice(games);

            return View("FakePayment", games);
        }

        public IActionResult RemoveFromCart(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var cart = CartHelper.GetCart(Request);
            cart.Remove((int)id);
            CartHelper.SaveCart(Response, cart);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ConfirmPayment()
        {
            var cart = CartHelper.GetCart(Request);
            var games = _context.Games.Where(g => cart.Contains(g.GameId)).ToList();

            var rentals = games.Select(g => new Rental(UserHelper.LoggedUserEmail, g.GameId, Enumerations.RentalStatus.ACTIVE, DateTime.Now));

            _context.Rentals.AddRange(rentals);
            foreach (var rental in rentals)
            {
                var game = _context.Games.FirstOrDefault(g => g.GameId == rental.GameId)!;
                game.QuantityInStock = Math.Clamp(game.QuantityInStock - 1, 0, game.MaxQuantity);
            }

            await _context.SaveChangesAsync();

            CartHelper.SaveCart(Response, new HashSet<int>());
            return RedirectToAction(nameof(Index));
        }

        private static double CalculateCartTotalPrice(List<Game> games)
        {
            if (games.Count == 0) return 0;

            var (chepestGame, cheapestGamePrice) = GetChepestGameAndItsPromotionPrice(games);
            var totalPrice = games.Aggregate(0.0, (acc, g) =>
                g == chepestGame ? acc + cheapestGamePrice : acc + g.DiscountedPrice);

            return totalPrice;
        }

        private static (Game, double) GetChepestGameAndItsPromotionPrice(List<Game> games)
        {
            var game = games.MinBy(g => g.DiscountedPrice)!;
            var price = game.DiscountedPrice * (games.Count >= 3 ? .7 : 1);
            return (game, price);
        }
    }
}