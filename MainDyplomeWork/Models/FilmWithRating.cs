using SmartReservationCinema.FilmContext;

namespace SmartReservationCinema.Models
{
    public class FilmWithRating
    {
        public Film Film { get; set; }
        public double? Rating { get; set; }
    }
}
