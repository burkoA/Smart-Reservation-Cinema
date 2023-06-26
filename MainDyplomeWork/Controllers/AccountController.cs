using MainDyplomeWork.FilmContext;
using MainDyplomeWork.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MainDyplomeWork.Controllers
{
    public class AccountController : Controller
    {
        private readonly FilmDbContext _db;

        public AccountController(FilmDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if(ModelState.IsValid)
            {
                User user = _db.Users.FirstOrDefault((User user) => user.Email == model.Email);
                if (user != null)
                {
                    if(user.Password == FilmContext.User.GetPasswordHash(model.Password))
                    {
                        InternalLogin(user);
                        return Redirect("~/Home/Index");
                    }
                }
                ModelState.AddModelError("", "Wrong password or email");
            }
            return View(model);
        }

        async private void InternalLogin(User user)
        {
            List<Claim> claim = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier,user.Email),
                            new Claim(ClaimTypes.Role,user.Role)
                        };
            ClaimsIdentity identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (ModelState.IsValid)
                try 
                {
                    User user = new User(model);
                    _db.Users.Add(user);
                    _db.SaveChanges();
                    InternalLogin(user);
                    return Redirect("~/Home/Index");
                }
                catch(System.Exception ex)
                {
                    ModelState.AddModelError("", "Valid for save");
                }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("~/Home/Index");
        }

        public IActionResult Denied()
        {
            return View();
        }
    }
}
