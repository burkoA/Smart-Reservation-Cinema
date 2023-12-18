using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
    public class Subtitle
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool IsSubtitle { get; set; }
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }
        public int FilmId { get; set; }
        [ForeignKey("FilmId")]
        public Film Film { get; set; }
    }
}
