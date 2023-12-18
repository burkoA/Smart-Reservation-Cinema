using SmartReservationCinema.FilmContext;

namespace SmartReservationCinema.Models
{
    public class CinemaDistanceModel : Cinema
    {
        public int Distance { get; set; } = 0;

        public CinemaDistanceModel()
        {

        }

        public CinemaDistanceModel(Cinema cinema,int distance)
        {
            Id = cinema.Id;
            CinemaName = cinema.CinemaName;
            Localisation = cinema.Localisation;
            LongCoordinate = cinema.LongCoordinate;
            LatCoordinate = cinema.LatCoordinate;
            CinemaRating = cinema.CinemaRating;
            Image = cinema.Image;
            TownId = cinema.TownId;
            Town = cinema.Town;
            this.Distance = distance;
        }
    }
}
