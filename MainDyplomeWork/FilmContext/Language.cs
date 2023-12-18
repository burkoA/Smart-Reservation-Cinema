using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.FilmContext
{
    public class Language
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public string LanguageName { get; set; }
    }
}
