using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.Models
{
    public class CommentModel
    {
        [Required]
        [MinLength(10)]
        [MaxLength(100)]
        public string Text { get; set; }
        [Required]
        public int IdFilm { get; set; }
    }
}
