using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
    public class Genre_Film
    {
        [Key]
        public int Id_Genre_Film { get; set; }
        public int Id_Film { get; set; }
        public int Id_Genre { get; set; }
        [ForeignKey("Id_Film")]
        public Film film { get; set; }
        [ForeignKey("Id_Genre")]
        public Genre genre { get; set; }

    }
}
