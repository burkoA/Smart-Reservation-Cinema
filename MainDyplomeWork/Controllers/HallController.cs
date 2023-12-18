using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class HallController : Controller
    {
        private readonly FilmDbContext _context;

        public HallController(FilmDbContext context)
        {
            _context = context;
        }
        // GET: HallController
        public async Task<IActionResult> Index()
        {
            return View(await _context.Halls.Include(h => h.Cinema).ToListAsync());
        }

        // GET: HallController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.Include(c => c.Cinema)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }
            GenerateList();
            return View(hall);
        }

        // GET: HallController/Create
        public ActionResult Create()
        {
            GenerateList();
            return View();
        }

        private void GenerateList()
        {
            ViewBag.Cinema = new SelectList(_context.Cinemas, "Id", "CinemaName");
        }

        // POST: HallController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hall hall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            GenerateList();
            return View(hall);
        }

        // GET: HallController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.FindAsync(id);
            GenerateList();
            Hall halls = _context.Halls.Include(h => h.Cinema).Where(c => c.Id == id).FirstOrDefault();
            if (halls == null)
            {
                return NotFound();
            }
            return View(halls);
        }

        // POST: HallController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Hall hall)
        {
            if (id != hall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallExists(hall.Id))
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
            GenerateList();
            return View(hall);
        }

        // GET: HallController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.Include(c => c.Cinema)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // POST: HallController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([FromForm] int id, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }

                Hall hall = _context.Halls.Where(c => c.Id == id).FirstOrDefault();

                if (hall == null)
                {
                    return View("Error", "Id not found");
                }

                _context.Halls.Remove(hall);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private bool HallExists(int id)
        {
            return _context.Halls.Any(e => e.Id == id);
        }
    }
}
