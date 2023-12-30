using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SystemWypozyczalniGier.Database;
using SystemWypozyczalniGier.Tables;
using SystemWypozyczalniGier.Enumerations;

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

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "Name", game.PublisherId);
            ViewData["Categories"] = new SelectList(_context.Categories.Where(x => x.GameId == game.GameId), "GameId", "Category");

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
        public async Task<IActionResult> Create(Game model, IFormFile mainPhoto, List<IFormFile> thumbnailPhotos, List<Category> categories)
        {
            if (ModelState.IsValid)
            {
                string mainFilePath, mainFileName;
                if (mainPhoto != null && mainPhoto.Length > 0)
                {
                    string guid = Guid.NewGuid().ToString("N");
                    string fileName = $"{guid}_0{Path.GetExtension(mainPhoto.FileName).ToLowerInvariant()}";

                    var rootPath = Path.GetFullPath("wwwroot");
                    var uploadPath = Path.Combine(rootPath, "uploads");
                    var filePath = Path.Combine(uploadPath, fileName);
                    mainFilePath = filePath;
                    mainFileName = fileName;
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
                        string guid = Guid.NewGuid().ToString("N");
                        string fileName = $"{guid}_{photoIndex}{Path.GetExtension(thumbnailPhoto.FileName).ToLowerInvariant()}";
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
                    using (var fileStream = new FileStream(mainFilePath, FileMode.Create))
                    {
                        mainPhoto.CopyTo(fileStream);
                        model.MainPhotoName = mainFileName;
                    }
                }
                else
                {
                    return RedirectToAction(nameof(Create));
                }

                _context.Add(model);
                await _context.SaveChangesAsync();

                if (categories != null && categories.Any())
                {
                    foreach (Category category in categories)
                    {
                        var entry = new GameCategory
                        {
                            GameId = model.GameId,
                            Category = category
                        };

                        _context.Add(entry);
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
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
            ViewData["Categories"] = new SelectList(_context.Categories.Where(x => x.GameId == game.GameId), "GameId", "Category");

            return View(game);
        }

        // POST: Games/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Game game, [FromForm] IFormFile? mainPhoto, List<IFormFile> thumbnailPhotos, List<Category> categories)
        {
            var existingGame = await _context.Games
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (existingGame == null)
            {
                return NotFound();
            }

            existingGame.PublisherId = game.PublisherId;
            existingGame.Title = game.Title;
            existingGame.Description = game.Description;
            existingGame.Price = game.Price;
            existingGame.Discount = game.Discount;
            existingGame.QuantityInStock = game.QuantityInStock;
            existingGame.MaxQuantity = game.MaxQuantity;
            existingGame.Accessibility = game.Accessibility;
            existingGame.Pegi = game.Pegi;



            if (ModelState.IsValid)
            {
                if (mainPhoto != null && mainPhoto.Length > 0)
                {
                   
                    var rp = Path.GetFullPath("wwwroot");
                    var up = Path.Combine(rp, "uploads");
                    var fp = Path.Combine(up, existingGame.MainPhotoName);

                    if (System.IO.File.Exists(fp))
                    {
                        System.IO.File.Delete(fp);
                    }
                

                    string guid = Guid.NewGuid().ToString("N");
                    string fileName = $"{guid}_0{Path.GetExtension(mainPhoto.FileName).ToLowerInvariant()}";

                    var rootPath = Path.GetFullPath("wwwroot");
                    var uploadPath = Path.Combine(rootPath, "uploads");
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        mainPhoto.CopyTo(fileStream);
                        existingGame.MainPhotoName = fileName;
                    }
                }

                if (thumbnailPhotos != null && thumbnailPhotos.Count > 0)
                {
                    foreach (string? fileName in new[] { existingGame.Photo1Name, existingGame.Photo2Name,
                        existingGame.Photo3Name, existingGame.Photo4Name})
                    {
                        if (!string.IsNullOrEmpty(fileName))
                        {
                            var rootPath = Path.GetFullPath("wwwroot");
                            var uploadPath = Path.Combine(rootPath, "uploads");
                            var filePath = Path.Combine(uploadPath, fileName);
                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                        }

                    }
                    existingGame.Photo2Name = null;
                    existingGame.Photo3Name = null;
                    existingGame.Photo4Name = null;
                    int photoIndex = 1;
                    foreach (var thumbnailPhoto in thumbnailPhotos)
                    {
                        string guid = Guid.NewGuid().ToString("N");
                        string fileName = $"{guid}_{photoIndex}{Path.GetExtension(thumbnailPhoto.FileName).ToLowerInvariant()}";
                        var rootPath = Path.GetFullPath("wwwroot");
                        var uploadPath = Path.Combine(rootPath, "uploads");
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            thumbnailPhoto.CopyTo(fileStream);
                            switch (photoIndex)
                            {
                                case 1:
                                    existingGame.Photo1Name = fileName;
                                    break;
                                case 2:
                                    existingGame.Photo2Name = fileName;
                                    break;
                                case 3:
                                    existingGame.Photo3Name = fileName;
                                    break;
                                default:
                                    existingGame.Photo4Name = fileName;
                                    break;
                            }

                        }
                        photoIndex++;
                    }
                }

                try
                {
                    _context.Update(existingGame);
                    await _context.SaveChangesAsync();

                    var categoriesToRemove = _context.Categories
                    .Where(category => category.GameId == existingGame.GameId)
                    .ToList();

                    _context.Categories.RemoveRange(categoriesToRemove);

                    foreach (Category category in categories)
                    {
                        var entry = new GameCategory
                        {
                            GameId = existingGame.GameId,
                            Category = category
                        };

                        _context.Add(entry);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(existingGame.GameId))
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
            return RedirectToAction(nameof(Edit));
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
