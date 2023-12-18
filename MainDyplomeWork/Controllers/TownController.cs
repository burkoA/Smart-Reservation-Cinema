using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class TownController : Controller
    {

        private readonly FilmDbContext _context;
        private string[] wordsToSearch = null;

        public TownController(FilmDbContext context)
        {
            _context = context;
        }

        // GET: TownController
        public async Task<IActionResult> Index([FromQuery] String search = "")
        {
            var items = await _context.Towns.ToListAsync();
            if (search != "")
            {
                wordsToSearch = SplitSearch(search);
                items = items.Where(searchCondition).ToList();
            }
            return View(items);
        }

        public bool searchCondition(Town town)
        {
            foreach (string word in wordsToSearch)
            {
                if (town.TownName.ToLower().Contains(word))
                    return true;
            }
            return false;
        }

        public String[] SplitSearch(String search)
        {
            return search.ToLower().Split(new char[] { ' ', ',', '.', '-', ':', ';', '\'', '"', '!', '?' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        // GET: TownController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var town = await _context.Towns
                .FirstOrDefaultAsync(m => m.Id == id);
            if (town == null)
            {
                return NotFound();
            }

            return View(town);
        }

        // GET: TownController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TownController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Town town)
        {
            if (ModelState.IsValid)
            {
                _context.Add(town);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(town);
        }

        // GET: TownController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var town = await _context.Towns.FindAsync(id);
            if (town == null)
            {
                return NotFound();
            }
            return View(town);
        }

        // POST: TownController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Town town)
        {
            if (id != town.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(town);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TownExists(town.Id))
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
            return View(town);
        }

        // GET: TownController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var town = await _context.Towns
                .FirstOrDefaultAsync(m => m.Id == id);
            if (town == null)
            {
                return NotFound();
            }

            return View(town);
        }

        // POST: TownController/Delete/5
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var town = await _context.Towns.FindAsync(id);
            _context.Towns.Remove(town);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TownExists(int id)
        {
            return _context.Towns.Any(e => e.Id == id);
        }
    }
}
