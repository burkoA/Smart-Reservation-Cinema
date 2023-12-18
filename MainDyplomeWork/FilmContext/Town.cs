using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.FilmContext
{
    public class Town
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TownName { get; set; }
        [Required]
        public string Region { get; set; }

        public List<Cinema> Cinemas { get; set; }
    }
}
