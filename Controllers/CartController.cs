using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SystemWypozyczalniGier.Database;

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
            var databaseContext = _context.Games
                .Include(g => g.Publisher)
                .Include(g => g.Reviews);
            return View(await databaseContext.ToListAsync());
        }
    }
}