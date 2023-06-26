using MainDyplomeWork.FilmContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MainDyplomeWork.Controllers
{
    public class FilmController : Controller
    {
        private readonly FilmDbContext _db;
        private readonly int itemsOnPage = 5;
        // GET: FilmController
        public FilmController(FilmDbContext context)
        {
            _db = context;
        }
        public ActionResult Index(int? id, int curPage=0)
        {
            ViewBag.GenresList = _db.Genres.ToList();
            IEnumerable<Film> items;
            
            if (id.HasValue && id.Value>0)
            {
                IEnumerable<Genre_Film> tmp = _db.GenresFilmes.Where(gf => gf.Id_Genre == id.Value);

                 items = _db.Films.Include(F => F.Genres).ThenInclude((Genre_Film gf) => gf.genre)
                    .Where(f => f.Genres.Contains(tmp.FirstOrDefault(gf => f.Id == gf.Id_Film)));
                //.ThenInclude((Genre_Film gf) => gf.genre)
            }
            else items = _db.Films.Include(F => F.Genres).ThenInclude((Genre_Film gf) => gf.genre);
            int itemCnt = items.Count();

            items = items.Skip(itemsOnPage * curPage).Take(itemsOnPage);

            
            int pageCnt = (int)Math.Ceiling(itemCnt / (double)itemsOnPage);
            ViewBag.pageCnt = pageCnt;
            ViewBag.curGenge = id??0;
            return View(items);
        }
        //public ActionResult Genre()
        //{
        //    ViewBag.GenresList = _db.Genres.ToList();
        //    return View(_db.Films.Include(F => F.Genre).Where(F =>F.);
        //}

        // GET: FilmController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FilmController/Create
        public ActionResult Create()
        {
            GenerateList();
            return View(new Film());
        }

        private void GenerateList()
        {
            ViewBag.Genres = _db.Genres.ToList();
            ViewBag.Directors = new SelectList(_db.Director, "Id_Director", "Name_Director");
        }

        // POST: FilmController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Film film, int[] genres)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                _db.Films.Add(film);
                _db.SaveChanges();
                foreach (int genreId in genres)
                {
                    _db.GenresFilmes.Add(new Genre_Film()
                    {
                        Id_Genre = genreId,
                        Id_Film = film.Id
                    });
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View(film);
            }
        }

        // GET: FilmController/Edit/5
        [Authorize(Roles ="admin,manager")]
        public ActionResult Edit(int id)
        {
            GenerateList();
            Film film = _db.Films.Include(f => f.Genres).Where(film => film.Id == id).FirstOrDefault();
            return View(film);
        }

        // POST: FilmController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id, Film film, int[] genres)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                _db.Films.Update(film);
                IEnumerable<Genre_Film> oldGenres = _db.GenresFilmes.Where(gf=>gf.Id_Film==film.Id).ToList();
                foreach (int genreId in genres)
                {
                    if (!oldGenres.Any(fg => fg.Id_Genre == genreId)){ 
                        _db.GenresFilmes.Add(new Genre_Film()
                        {
                            Id_Genre = genreId,
                            Id_Film = film.Id
                        });
                    }
                }
                foreach(Genre_Film gengeFilm in oldGenres)
                {
                    if (!genres.Contains(gengeFilm.Id_Genre))
                    {
                        _db.GenresFilmes.Remove(gengeFilm);
                    }
                }                
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                GenerateList();
                return View();
            }
        }

        // GET: FilmController/Delete/5
        public ActionResult Delete(int id)
        {
            Film film = _db.Films.Where(film => film.Id == id).FirstOrDefault();
            return View(film);
        }

        // POST: FilmController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([FromForm] int id, IFormCollection collection)
        {
            try
            {
                if (!ModelState.IsValid) { throw new Exception(""); }
                Film film = _db.Films.Where(film => film.Id == id).FirstOrDefault();
                if(film==null)
                {
                    return View("Error","Id not found");
                }
                _db.Films.Remove(film);
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
