using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SystemWypozyczalniGier.Database;
using SystemWypozyczalniGier.Enumerations;
using SystemWypozyczalniGier.Helpers;
using SystemWypozyczalniGier.Models;
using SystemWypozyczalniGier.Tables;

namespace SystemWypozyczalniGier.Controllers
{
    public class ShopController : Controller
    {
        private readonly DatabaseContext _context;

        public ShopController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? title, List<Category> categories)
        {
            var databaseContext = from games in _context.Games
                                  .Include(g => g.Publisher)
                                  select games;

            ViewData["TitleFilter"] = title;
            ViewData["CategoryFilter"] = "";

            if (!string.IsNullOrEmpty(title))
            {
                databaseContext = databaseContext
                    .Where(g => g.Title.Contains(title));
            }

            if (categories.Count > 0)
            {
                foreach (var category in categories)
                {
                    databaseContext = databaseContext
                        .Where(g => g.Categories
                        .Select(c => c.Category)
                        .Contains(category));

                    ViewData["CategoryFilter"] += $"{category}, ";
                }

                ViewData["CategoryFilter"] = ((string)ViewData["CategoryFilter"])
                    .Trim()
                    .Remove(((string)ViewData["CategoryFilter"]).Length - 2);
            }

            var model = new GameViewModel()
            {
                Games = await databaseContext.ToListAsync(),
                FilterCategories = GameViewModel.AllCategories.Except(categories).ToList() //categories.Count == 0 ? GameViewModel.AllCategories : categories
            };

            return View(model);
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