using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SystemWypozyczalniGier.Database;
using SystemWypozyczalniGier.Tables;

namespace SystemWypozyczalniGier.Controllers
{
    public class RentalsController : Controller
    {
        private readonly DatabaseContext _context;

        public RentalsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Rentals.Include(r => r.Account).Include(r => r.Game);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Rentals/Details
        public async Task<IActionResult> Details([FromQuery(Name = "accountEmail")] string? accountEmail, [FromQuery(Name = "gameId")] int? gameId)
        {
            if (accountEmail == null || gameId == null || _context.Rentals == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Account)
                .Include(r => r.Game)
                .FirstOrDefaultAsync(m => m.AccountEmail == accountEmail && m.GameId == gameId);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewData["AccountEmail"] = new SelectList(_context.Accounts, "Email", "Email");
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title");
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountEmail,GameId,RentalStatus,RentalTime")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rental);
                var game = _context.Games.FirstOrDefault(g => g.GameId == rental.GameId)!;
                game.QuantityInStock = Math.Clamp(game.QuantityInStock - 1, 0, game.MaxQuantity);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountEmail"] = new SelectList(_context.Accounts, "Email", "Email", rental.AccountEmail);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", rental.GameId);
            return View(rental);
        }

        // GET: Rentals/Edit
        public async Task<IActionResult> Edit([FromQuery(Name = "accountEmail")] string? accountEmail, [FromQuery(Name = "gameId")] int? gameId)
        {
            if (accountEmail == null || gameId == null || _context.Rentals == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals.FindAsync(accountEmail, gameId);
            if (rental == null)
            {
                return NotFound();
            }
            ViewData["AccountEmail"] = new SelectList(_context.Accounts, "Email", "Email", rental.AccountEmail);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", rental.GameId);
            return View(rental);
        }

        // POST: Rentals/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string accountEmail, int gameId, [Bind("AccountEmail,GameId,RentalStatus,RentalTime")] Rental rental)
        {
            if (accountEmail != rental.AccountEmail || gameId != rental.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.AccountEmail, rental.GameId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountEmail"] = new SelectList(_context.Accounts, "Email", "Email", rental.AccountEmail);
            ViewData["GameId"] = new SelectList(_context.Games, "GameId", "Title", rental.GameId);
            return View(rental);
        }

        // GET: Rentals/Delete
        public async Task<IActionResult> Delete([FromQuery(Name = "accountEmail")] string? accountEmail, [FromQuery(Name = "gameId")] int? gameId)
        {
            if (accountEmail == null || gameId == null || _context.Rentals == null)
            {
                return NotFound();
            }

            var rental = await _context.Rentals
                .Include(r => r.Account)
                .Include(r => r.Game)
                .FirstOrDefaultAsync(m => m.AccountEmail == accountEmail && m.GameId == gameId);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string accountEmail, int gameId)
        {
            if (_context.Rentals == null)
            {
                return Problem("Entity set 'DatabaseContext.Rentals'  is null.");
            }
            var rental = await _context.Rentals.FindAsync(accountEmail, gameId);
            if (rental != null)
            {
                var game = _context.Games.FirstOrDefault(g => g.GameId == rental.GameId)!;
                game.QuantityInStock = Math.Clamp(game.QuantityInStock + 1, 0, game.MaxQuantity);
                _context.Rentals.Remove(rental);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(string accountEmail, int gameId)
        {
          return (_context.Rentals?.Any(e => e.AccountEmail == accountEmail && e.GameId == gameId)).GetValueOrDefault();
        }
    }
}
