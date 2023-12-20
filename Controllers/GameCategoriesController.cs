using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SystemWypozyczalniGier.Database;
using SystemWypozyczalniGier.Enumerations;
using SystemWypozyczalniGier.Tables;

namespace SystemWypozyczalniGier.Controllers
{
    public class GameCategoriesController : Controller
    {
        private readonly DatabaseContext _context;

        public GameCategoriesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: GameCategories
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Categories.Include(g => g.Game);
            return View(await databaseContext.ToListAsync());
        }

        // GET: GameCategories/Details
        public async Task<IActionResult> Details([FromQuery(Name = "gameId")] int? gameId, [FromQuery(Name = "category")] Category? category)
        {
            if (gameId == null || category == null || _context.Categories == null)
            {
                return NotFound();
            }

            var gameCategory = await _context.Categories
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.GameId == gameId && m.Category == category);
            if (gameCategory == null)
            {
                return NotFound();
            }

            return View(gameCategory);
        }

        // GET: GameCategories/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title");
            return View();
        }

        // POST: GameCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,Category")] GameCategory gameCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", gameCategory.GameId);
            return View(gameCategory);
        }

        // GET: GameCategories/Delete
        public async Task<IActionResult> Delete([FromQuery(Name = "gameId")] int? gameId, [FromQuery(Name = "category")] Category? category)
        {
            if (gameId == null || category == null || _context.Categories == null)
            {
                return NotFound();
            }

            var gameCategory = await _context.Categories
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.GameId == gameId && m.Category == category);
            if (gameCategory == null)
            {
                return NotFound();
            }

            return View(gameCategory);
        }

        // POST: GameCategories/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? gameId, Category? category)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'DatabaseContext.Categories'  is null.");
            }
            var gameCategory = await _context.Categories.FindAsync(gameId, category);
            if (gameCategory != null)
            {
                _context.Categories.Remove(gameCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
