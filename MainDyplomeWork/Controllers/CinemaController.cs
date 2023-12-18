using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;
using SmartReservationCinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartReservationCinema.Controllers
{
    public class CinemaController : Controller
    {
        private readonly FilmDbContext _db;
        private readonly int itemsOnPage = 5;
        private readonly IWebHostEnvironment _env;
        private string[] wordsToSearch = null;
        public CinemaController(FilmDbContext context, IWebHostEnvironment env)
        {
            _db = context;
            _env = env;
        }

        // GET: CinemaController
        public ActionResult Index(int? id, int curPage=0,[FromQuery] String search="")
        {
            ViewBag.TownsList = _db.Towns.ToList();
            IEnumerable<Cinema> items;

            if (id.HasValue && id.Value > 0)
            {
                IEnumerable<Town> tmp = _db.Towns.Where(tc => tc.Id == id.Value);

                items = _db.Cinemas.Include(c => c.Town)
                    .Where(c => c.Town.Equals(tmp.FirstOrDefault(tc => c.TownId == tc.Id)));


            }
            else items = _db.Cinemas.Include(C => C.Town).ThenInclude((Town tc) => tc.Cinemas).ToList();
            if (search != "")
            {
                wordsToSearch = SplitSearch(search);
                items = items.Where(searchCondition);
            }
            int itemCnt = items.Count();

            items = items.Skip(itemsOnPage * curPage).Take(itemsOnPage);

            int pageCnt = (int)Math.Ceiling(itemCnt / (double)itemsOnPage);
            ViewBag.pageCnt = pageCnt;
            ViewBag.curGenge = id ?? 0;
            return View(items);
        }

        public bool searchCondition(Cinema cinema)
        {
            foreach (string word in wordsToSearch)
            {
                if (cinema.CinemaName.ToLower().Contains(word))
                    return true;
            }
            return false;
        }

        public String[] SplitSearch(String search)
        {
            return search.ToLower().Split(new char[] { ' ', ',', '.', '-', ':', ';', '\'', '"', '!', '?' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        // GET: CinemaController/Details/5
        public ActionResult Details(int id, DateTime? filterDate=null)
        {
            if(filterDate == null)
            {
                filterDate = DateTime.Today;
            }
            ViewBag.filterDate = filterDate;
            Cinema cinema = _db.Cinemas.Include(t => t.Town).FirstOrDefault(c => c.Id == id);
            ViewBag.Sessions = _db.Sessions.Where(s => s.CinemaId == id && s.StartDate<=filterDate && s.EndDate>=filterDate)
                .Include(s => s.film).Include(s => s.Language).Include(s => s.hall).ToArray();
            return View(cinema);
        }

        // GET: CinemaController/Create
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create()
        {
            GenerateList();
            return View(new CinemaModel());
        }

        private void GenerateList()
        {
            ViewBag.Towns = new SelectList(_db.Towns, "Id", "TownName");
        }

        // POST: CinemaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create(CinemaModel cinema)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                if (cinema.NewImage != "")
                {
                    cinema.Image = cinema.NewImage;
                }
                else
                {
                    cinema.Image = "DefaultImage.jpg";
                }
                _db.Cinemas.Add(cinema);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View(cinema);
            }
        }

        // GET: CinemaController/Edit/5
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id)
        {
            GenerateList();
            Cinema cinema = _db.Cinemas.Include(c => c.Town).Where(cinema => cinema.Id == id).FirstOrDefault();
            return View(new CinemaModel(cinema));
        }

        // POST: CinemaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id, CinemaModel cinema)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }

                if (cinema.NewImage != "")
                {
                    string imgFolder = _env.WebRootPath + "/img/cinemaImage/";
                    if (!String.IsNullOrEmpty(cinema.Image))
                    {
                        System.IO.File.Delete(imgFolder + cinema.Image);
                    }
                    cinema.Image = cinema.NewImage;
                }

                _db.Cinemas.Update(cinema);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View(cinema);
            }
        }

        // GET: CinemaController/Delete/5
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete(int id)
        {
            Cinema cinema = _db.Cinemas.Where(c => c.Id == id).FirstOrDefault();
            return View(cinema);
        }

        // POST: CinemaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete([FromForm]int id, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception("");
                }

                Cinema cinema = _db.Cinemas.Where(c => c.Id == id).FirstOrDefault();

                if (cinema == null)
                {
                    return View("Error", "Id not found");
                }

                _db.Cinemas.Remove(cinema);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
