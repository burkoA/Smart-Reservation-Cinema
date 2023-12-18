using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
    public class Film_Actor
    {
        [Key]
        public int Id_Film_Actor { get; set; }
        public int Id_Actor { get; set; }
        public int Id_Film { get; set; }
        [ForeignKey("Id_Actor")]
        public Actor actor { get; set; }
        [ForeignKey("Id_Film")]
        public Film film { get; set; }
    }
}
