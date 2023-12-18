using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using SmartReservationCinema.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SmartReservationCinema.Controllers
{
    public class CalendarController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EventWarning(EventModel eventModel)
        {
            if(ModelState.IsValid)
            {
                return View(eventModel);
            } else
            {
                return RedirectToAction("Error","Home");
            }
        }

        //[HttpGet]
        //public IActionResult EventCreate()
        //{
        //    return View();
        //}

        [HttpGet]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [GoogleScopedAuthorize(CalendarService.ScopeConstants.CalendarEvents, CalendarService.ScopeConstants.Calendar)]
        public async Task<IActionResult> EventCreate(EventModel eventModel, [FromServices] IGoogleAuthProvider auth)
        {
            if (ModelState.IsValid)
            {
                GoogleCredential cred = await auth.GetCredentialAsync();
                var service = new CalendarService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = cred
                });

                CalendarList calendars = await service.CalendarList.List().ExecuteAsync();
                CalendarListEntry calEntry = calendars.Items.Where((CalendarListEntry calItem) => calItem.Primary == true).FirstOrDefault();
                string calendarId = calEntry.Id;
                //Events events = await service.Events.List(calendarId).ExecuteAsync();
                Event ev = new Event()
                {
                    Start = new EventDateTime() { DateTime = eventModel.StartDateTime },
                    End = new EventDateTime() { DateTime = eventModel.EndDateTime },
                    Description = eventModel.CinemaName,
                    Summary = eventModel.FilmName
                };
                Event ev2 = await service.Events.Insert(ev, calendarId).ExecuteAsync();
                //IEnumerable<string> list = events.Items//.Where((Event ev) => ev.Start.DateTime.HasValue && ev.Start.DateTime.Value.Date.CompareTo(DateTime.Today)>=0)
                //            .Select((Event ev) => ev.Start.DateTime?.ToString() + " " + ev.Summary)
                //            .ToList();

                return View("EventAddOk");
            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
