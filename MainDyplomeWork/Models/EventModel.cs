using System;

namespace SmartReservationCinema.Models
{
    public class EventModel
    {
        public string CinemaName { get; set; }
        public string FilmName { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StartDateTime { get { return StartDate+new TimeSpan(StartTime.Hour,StartTime.Minute,0); } }
        public DateTime EndDateTime { get { return StartDateTime + new TimeSpan(DurationFilm/60,DurationFilm%60,0); } }
        public int DurationFilm { get; set; } 
    }
}
