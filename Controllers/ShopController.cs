using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemWypozyczalniGier.Database;

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
                .Include(g => g.Publisher)
                .Include(g => g.Reviews)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            // Œrednia ocena
            ViewBag.AverageMark = game.Reviews.Count > 0 ?
                (float)game.Reviews.Aggregate(0, (acc, r) => acc + r.Mark) / game.Reviews.Count :
                5;

            return View(game);
        }
    }
}