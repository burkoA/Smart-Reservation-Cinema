using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MainDyplomeWork.FilmContext
{
    public class Film
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(30)]
        public string Description { get; set; } = "";
        [Required]
        [MaxLength(100)]
        public string FilmName { get; set; } = "";
        public int Time { get; set; }
        [MaxLength(50)]
        public string Image { get; set; } = "";
        [Required]
        [DataType(DataType.Date)]
        public DateTime Realese { get; set; }
        public IEnumerable<Genre_Film> Genres { get; set; }=new List<Genre_Film>();
        public int DirectorId { get; set; }
        [ForeignKey("DirectorId")]
        public Director Director { get; set; }
    }
}
