using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(100)]
        public string Text { get; set; }
        [Required]
        public int IdFilm { get; set; }
        [Required]
        public int IdUser { get; set; }
        [ForeignKey("IdFilm")]
        public Film Film { get; set; }
        [ForeignKey("IdUser")]
        public User User { get; set; }

    }
}
