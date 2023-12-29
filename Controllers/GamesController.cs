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
    public class GamesController : Controller
    {
        private readonly DatabaseContext _context;

        public GamesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var databaseContext = _context.Games.Include(g => g.Publisher);
            return View(await databaseContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Publisher)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "Name");
            var model = new Game
            {
                MainPhotoName = "defaultMainPhotoName",
                Photo1Name = "defaultPhoto1Name"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Game model, IFormFile mainPhoto, List<IFormFile> thumbnailPhotos)
        {
            if (mainPhoto != null && mainPhoto.Length > 0)
            {   
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileName = $"{timestamp}_0{Path.GetExtension(mainPhoto.FileName).ToLowerInvariant()}";

                var rootPath = Path.GetFullPath("wwwroot");
                var uploadPath = Path.Combine(rootPath, "uploads");
                var filePath = Path.Combine(uploadPath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    mainPhoto.CopyTo(fileStream);
                    model.MainPhotoName = fileName;
                }
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }

            if (thumbnailPhotos != null && thumbnailPhotos.Count > 0)
            {
                int photoIndex = 1;
                foreach (var thumbnailPhoto in thumbnailPhotos)
                {   
                    string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string fileName = $"{timestamp}_{photoIndex}{Path.GetExtension(thumbnailPhoto.FileName).ToLowerInvariant()}";
                    var rootPath = Path.GetFullPath("wwwroot");
                    var uploadPath = Path.Combine(rootPath, "uploads");
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        thumbnailPhoto.CopyTo(fileStream);
                        switch (photoIndex)
                        {
                            case 1:
                                model.Photo1Name = fileName;
                                break;
                            case 2:
                                model.Photo2Name = fileName;
                                break;
                            case 3:
                                model.Photo3Name = fileName;
                                break;
                            default:
                                model.Photo4Name = fileName;
                                break;
                        }

                    }
                    photoIndex++;
                }
            }
            else
            {
                return RedirectToAction(nameof(Create));
            }

            if (ModelState.IsValid)
            {
                var game = new Game
                {
                    PublisherId = model.PublisherId,
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    QuantityInStock = model.QuantityInStock,
                    MaxQuantity = model.MaxQuantity,
                    Accessibility = model.Accessibility,
                    Discount = model.Discount,
                    Pegi = model.Pegi,
                    MainPhotoName = model.MainPhotoName,
                    Photo1Name = model.Photo1Name,
                    Photo2Name = model.Photo2Name,
                    Photo3Name = model.Photo3Name,
                    Photo4Name = model.Photo4Name,
                };

                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //
            foreach (var key in ModelState.Keys)
            {
                var modelStateEntry = ModelState[key];
                foreach (var error in modelStateEntry.Errors)
                {
                    Console.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                }
            }
            //
            return RedirectToAction(nameof(Create));
        }



        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "Name", game.PublisherId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,PublisherId,Title,Description,Price,QuantityInStock,MaxQuantity,Accessibility,Discount,Pegi")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
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
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "Name", game.PublisherId);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Games == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Publisher)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Games == null)
            {
                return Problem("Entity set 'DatabaseContext.Games'  is null.");
            }
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return (_context.Games?.Any(e => e.GameId == id)).GetValueOrDefault();
        }
    }
}
