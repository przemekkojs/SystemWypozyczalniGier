using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using SystemWypozyczalniGier.Database;
using SystemWypozyczalniGier.Helpers;

namespace SystemWypozyczalniGier.Controllers
{
    public class ShopController : Controller
    {
        private readonly DatabaseContext _context;

        public ShopController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Games.Include(g => g.Publisher);
            return View(await databaseContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Rentals)
                .Include(g => g.Reviews)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            ViewBag.AverageMark = game.Reviews.Count > 0 ?
                (float)game.Reviews.Aggregate(0, (acc, r) => acc + r.Mark) / game.Reviews.Count : 5;
            ViewBag.InCart = Request.Cookies["cart"] != null &&
                (JsonConvert.DeserializeObject<HashSet<int>>(Request.Cookies["cart"]!)?.Contains((int)id) ?? false);

            var activeRental = game.Rentals.FirstOrDefault(r => r.AccountEmail == UserHelper.LoggedUserEmail && r.RentalStatus == Enumerations.RentalStatus.ACTIVE);
            ViewBag.RentalEnd = activeRental != null ? activeRental.RentalTime.AddDays(30).ToString("dd.MM.yyyy") : "";



            return View(game);
        }

        public IActionResult AddToCart(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var cart = CartHelper.GetCart(Request);
            cart.Add((int)id);
            CartHelper.SaveCart(Response, cart);

            return RedirectToAction(nameof(CartController.Index), nameof(CartController).Replace("Controller", ""));
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

            return RedirectToAction(nameof(CartController.Index), nameof(CartController).Replace("Controller", ""));
        }
    }
}