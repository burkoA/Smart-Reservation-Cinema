using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.FilmContext
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string GenreName { get; set; }

        public List<Genre_Film> films { get; set; }
    }
}
