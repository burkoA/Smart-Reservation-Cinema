using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CinemaName { get; set; }
        [Required]
        public string Localisation { get; set; }
        public double LongCoordinate { get; set; }
        public double LatCoordinate { get; set; }
        [Required]
        public double CinemaRating { get; set; }
        public string Image { get; set; } = "";
        //public IEnumerable<Town> TownsCinema { get; set; } = new List<Town>();
        public int TownId { get; set; }
        [ForeignKey("TownId")]
        public Town Town { get; set; }
        public List<Session> Sessions { get; set; } = new List<Session>();
    }
}
