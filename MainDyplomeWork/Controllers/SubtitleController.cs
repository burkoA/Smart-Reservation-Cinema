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
    public class SubtitleController : Controller
    {
        private readonly FilmDbContext _db;

        public SubtitleController(FilmDbContext context)
        {
            _db = context;
        }
        // GET: SubtitleController
        public async Task<IActionResult> Index()
        {
            return View(await _db.Subtitles.Include(f => f.Film).Include(l => l.Language).ToListAsync());
        }

        // GET: SubtitleController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtitle = await _db.Subtitles.Include(f => f.Film).Include(l => l.Language)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subtitle == null)
            {
                return NotFound();
            }
            GenerateList();
            return View(subtitle);
        }

        // GET: SubtitleController/Create
        public ActionResult Create()
        {
            GenerateList();
            return View();
        }

        private void GenerateList()
        {
            ViewBag.Languages = new SelectList(_db.Languages, "Id", "LanguageName");
            ViewBag.Films = new SelectList(_db.Films, "Id", "FilmName");
        }

        // POST: SubtitleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subtitle subtitle)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                _db.Subtitles.Add(subtitle);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View(subtitle);
            }
        }

        // GET: SubtitleController/Edit/5
        public ActionResult Edit(int id)
        {
            GenerateList();
            Subtitle subtitle = _db.Subtitles.Include(s => s.Language).Where(subtitle => subtitle.Id == id)
                .Include(s => s.Film).FirstOrDefault();
            return View(subtitle);
        }

        // POST: SubtitleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Subtitle subtitle)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }

                _db.Subtitles.Update(subtitle);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View();
            }
        }

        // GET: SubtitleController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtitle = await _db.Subtitles.Include(f => f.Film).Include(l => l.Language)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subtitle == null)
            {
                return NotFound();
            }

            return View(subtitle);
        }

        // POST: SubtitleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }

                Subtitle subtitle = _db.Subtitles.Where(s => s.Id == id).FirstOrDefault();

                if(subtitle == null)
                {
                    return View("Error", "Id not found");
                }

                _db.Subtitles.Remove(subtitle);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View();
            }
        }
    }
}
