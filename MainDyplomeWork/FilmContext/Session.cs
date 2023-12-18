using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Time)]
        public DateTime StartTime { get; set; }
        public int CinemaId { get; set; }
        [ForeignKey("CinemaId")]
        public Cinema cinema { get; set; }
        public int? HallId { get; set; }
        [ForeignKey("HallId")]
        public Hall? hall { get; set; }
        public int FilmId { get; set; }
        [ForeignKey("FilmId")]
        public Film film { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }
        public int SubtitleLanguageId { get; set; }
        [ForeignKey("SubtitleLanguageId")]
        public Language subtitleLanguage;
    }
}
