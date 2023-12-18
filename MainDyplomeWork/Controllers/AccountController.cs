using SmartReservationCinema.FilmContext;
using SmartReservationCinema.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SmartReservationCinema.Services;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace SmartReservationCinema.Controllers
{
    public class AccountController : Controller
    {
        private readonly FilmDbContext _db;
        private readonly MailSender _mailSender;
        private string[] wordsToSearch = null;

        public AccountController(FilmDbContext context, MailSender mailSender)
        {
            _db = context;
            _mailSender = mailSender;
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
            if (ModelState.IsValid)
            {
                User user = _db.Users.FirstOrDefault((User user) => user.Email == model.Email);
                if (user != null)
                {
                    if (user.Password == FilmContext.User.GetPasswordHash(model.Password))
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
                            new Claim(ClaimTypes.Name,user.Email),
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
                catch (System.Exception ex)
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

        [HttpGet]
        public IActionResult Profile()
        {
            User user = GetCurrentUser(_db, HttpContext);
            if(user == null)
            {
                return View("Error", new ErrorViewModel() { RequestId = "Something went wrong!"});
            }
            ProfileModel profile = new ProfileModel(user);
            return View(profile);
        }

        [HttpPost]
        public IActionResult Profile(ProfileModel profile)
        {
            if(ModelState.IsValid)
            {
                User user1 = GetCurrentUser(_db, HttpContext);
                user1.Update(profile);
                _db.Update(user1);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(profile);
        }

        public static User? GetCurrentUser(FilmDbContext db, HttpContext httpContext)
        {
            return db.Users.Where(u => u.Email == httpContext.User.Identity.Name).FirstOrDefault();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(ChangePasswordModel changePassword)
        {
            if (ModelState.IsValid)
            {
                User user = GetCurrentUser(_db, HttpContext);
                string oldPasswordHashed = SmartReservationCinema.FilmContext.User.GetPasswordHash(changePassword.OldPassword);
                if( oldPasswordHashed != user.Password)
                {
                    ModelState.AddModelError("OldPassword", "Old password is wrong!");
                    return View(changePassword);
                }
                user.Password = SmartReservationCinema.FilmContext.User.GetPasswordHash(changePassword.NewPassword);
                _db.Users.Update(user);
                _db.SaveChanges();
                return RedirectToAction("PasswordAlreadyChanged");
            }
            return View(changePassword);
        }

        public IActionResult Denied()
        {
            return View();
        }

        public async Task<IActionResult> PasswordRestore()
        {
            //_mailSender.SendMessage("arsenburko67@gmail.com", "Mail Test", "Hello Freund");
            return View("RestorePassword");
        }

        [HttpPost]
        public async Task<IActionResult> PasswordRestore(PasswordRestore passwordRestore)
        {
            if (ModelState.IsValid)
            {
                User user = _db.Users.Where(u => u.Email == passwordRestore.Email).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("Email", "This email doesn't exist");
                    return View(passwordRestore);
                }
                Random random = new Random();
                int codeGenerate = random.Next(100000, 999999);
                //user.RestoreCode = codeGenerate;
                //_db.Update(user);
                HttpContext.Session.SetInt32("RestoreCode", codeGenerate);
                HttpContext.Session.SetString("CodeTime", DateTime.Now.AddMinutes(15).Ticks.ToString());
                HttpContext.Session.SetString("Email", user.Email);
                HttpContext.Session.SetInt32("CodeIsRight", 0);
                _mailSender.SendMessage(user.Email, "Restore Code Smart Reservation",
                    "Your generate code " + codeGenerate);
                return RedirectToAction("CheckRestoreCode");
            }
            return View("RestorePassword");
        }

        [HttpGet]
        public async Task<IActionResult> CheckRestoreCode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckRestoreCode(PasswordRestore passwordRestore)
        {
            if (String.IsNullOrEmpty(passwordRestore.RestoreCode))
            {
                ModelState.AddModelError("RestoreCode", "Restore code cannot be empty!");
                return View(passwordRestore);
                HttpContext.Session.SetInt32("CodeIsRight", 0);
            }
            int code;
            int rightCode = HttpContext.Session.GetInt32("RestoreCode").Value;
            long CodeTime = long.Parse(HttpContext.Session.GetString("CodeTime"));
            if (rightCode == 0 || DateTime.Now.Ticks > CodeTime)
            {
                ModelState.AddModelError("RestoreCode", "Something went wrong!");
                HttpContext.Session.SetInt32("CodeIsRight", 0);
                return View(passwordRestore);
            }

            if (!Int32.TryParse(passwordRestore.RestoreCode, out code))
            {
                ModelState.AddModelError("RestoreCode", "Restore code not value!");
                HttpContext.Session.SetInt32("CodeIsRight", 0);
                return View(passwordRestore);

            }
            if (code != rightCode)
            {
                ModelState.AddModelError("RestoreCode", "Restore code is not correct!");
                HttpContext.Session.SetInt32("CodeIsRight", 0);
                return View(passwordRestore);
            }
            HttpContext.Session.SetInt32("CodeIsRight", 1);
            return RedirectToAction("EnterNewPassword");

        }

        [HttpGet]
        public async Task<IActionResult> EnterNewPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnterNewPassword(EnterNewPassword newPassword)
        {
            if (!ModelState.IsValid)
            {
                return View(newPassword);
            }
            String userEmail = HttpContext.Session.GetString("Email");
            int codeIsRight = HttpContext.Session.GetInt32("CodeIsRight") ?? 0;
            if (userEmail == null || codeIsRight != 1)
            {
                ModelState.AddModelError("", "Error work!");
                return View(newPassword);
            }
            User user = _db.Users.Where(u => u.Email == userEmail).FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("", "Something went wrong!");
                return View(newPassword);
            }
            user.Password = SmartReservationCinema.FilmContext.User.GetPasswordHash(newPassword.Password);
            _db.Users.Update(user);
            _db.SaveChanges();
            return RedirectToAction("PasswordAlreadyChanged");
        }

        [HttpGet]
        public async Task<IActionResult> PasswordAlreadyChanged()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AccountList([FromQuery] String search = "")
        {
            var items = await _db.Users.ToListAsync();
            if (search != "")
            {
                wordsToSearch = SplitSearch(search);
                items = items.Where(searchCondition).ToList();
            }
            return View(items);
        }

        public bool searchCondition(User user)
        {
            foreach (string word in wordsToSearch)
            {
                if (user.Email.ToLower().Contains(word))
                    return true;
            }
            return false;
        }

        public String[] SplitSearch(String search)
        {
            return search.ToLower().Split(new char[] { ' ', ',', '.', '-', ':', ';', '\'', '"', '!', '?' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    var selectedRoles = new List<string>();
                    if (user.IsAdminSelected) selectedRoles.Add("admin");
                    if (user.IsUserSelected) selectedRoles.Add("user");
                    if (user.IsManagerSelected) selectedRoles.Add("manager");

                    user.Role = string.Join(",", selectedRoles);

                    _db.Update(user);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }   
                }
                return RedirectToAction(nameof(AccountList));
            }
            return View(user);
        }

        private bool UserExists(int id)
        {
            return _db.Users.Any(e => e.Id == id);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(AccountList));
        }
    }
}
