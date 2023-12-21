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
            var games = _context.Games
                .Where(g => cart.Contains(g.GameId))
                .Include(g => g.Publisher)
                .Include(g => g.Reviews);

            return View(await games.ToListAsync());
        }

        public async Task<IActionResult> Payment()
        {
            var cart = CartHelper.GetCart(Request);
            var games = _context.Games
                .Where(g => cart.Contains(g.GameId))
                .Include(g => g.Publisher)
                .Include(g => g.Reviews);

            return View("FakePayment", await games.ToListAsync());
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
            await _context.SaveChangesAsync();

            CartHelper.SaveCart(Response, new HashSet<int>());
            return RedirectToAction(nameof(Index));
        }
    }
}