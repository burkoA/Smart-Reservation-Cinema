using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;
using System;
using System.Linq;

namespace SmartReservationCinema.Controllers
{
    public class SessionController : Controller
    {

        private readonly FilmDbContext _context;

        public SessionController(FilmDbContext context)
        {
            _context = context;
        }

        // GET: SessionController
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: SessionController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: SessionController/Create
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create(int cinemaId)
        {
            //ViewBag.cinemaId = cinemaId;
            GenerateList();
            return View(new Session() { CinemaId=cinemaId});
        }

        private void GenerateList()
        {
            ViewBag.CinemaList = new SelectList(_context.Cinemas, "Id", "CinemaName");
            ViewBag.HallList = new SelectList(_context.Halls, "Id", "HallName");
            ViewBag.FilmList = new SelectList(_context.Films, "Id", "FilmName");
            ViewBag.LanguageList = new SelectList(_context.Languages, "Id", "LanguageName");
        }

        // POST: SessionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Create([FromForm]Session session)
        {
            try
            {
                if (!ModelState.IsValid) { return Exception(""); }
                //session.CinemaId = 1;
                _context.Sessions.Add(session);
                _context.SaveChanges();
                return RedirectToAction("Details","Cinema",new {id=session.CinemaId});
            }
            catch
            {
                GenerateList();
                return View();
            }
        }

        private ActionResult Exception(string v)
        {
            throw new NotImplementedException();
        }

        // GET: SessionController/Edit/5
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit(int id)
        {
            GenerateList();
            Session session = _context.Sessions.Where(s => s.Id == id).FirstOrDefault();
            return View(session);
        }

        // POST: SessionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Edit([FromRoute]int id,[FromForm] Session session)
        {
            try
            {
                _context.Sessions.Update(session);
                _context.SaveChanges();
                return RedirectToAction("Details", "Cinema", new { id = session.CinemaId });
            }
            catch
            {
                GenerateList();
                return View(session);
            }
        }

        // GET: SessionController/Delete/5
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete(int id)
        {
            Session session = _context.Sessions.Include(s => s.cinema).Include(s => s.hall).Include(s => s.film)
                .Include(s => s.Language).FirstOrDefault(s => s.Id == id);
            return View(session);
        }

        // POST: SessionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public ActionResult Delete([FromRoute]int id, Session session)
        {
            try
            {
                _context.Sessions.Remove(session);
                _context.SaveChanges();
                return RedirectToAction("Details", "Cinema", new { id = session.CinemaId });
            }
            catch
            {
                return View(session);
            }
        }


    }
}
