using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
    public class FilmMark
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1,5)]
        public int Mark { get; set; }
        public DateTime MarkDate { get; set; } = DateTime.Now;
        [Required]
        public int UserId { get; set; }
        [Required]
        public int FilmId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("FilmId")]
        public Film Film { get; set; }
    }
}
