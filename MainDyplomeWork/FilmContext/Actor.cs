using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.FilmContext
{
    public class Actor
    {
        [Key]
        public int Id_Actor { get; set; }
        [Required]
        public string Actor_Name { get; set; } = "";
        [Required]
        public string Nationality { get; set; } = "";
        public List<Film_Actor> Films { get; set; }
    }
}
