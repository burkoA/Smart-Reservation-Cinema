using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainDyplomeWork.FilmContext
{
    public class Director
    {
        [Key]
        public int Id_Director { get; set; }
        [Required]
        public string Name_Director { get; set; } = "";
        [Required]
        public string Birth_Place { get; set; } = "";
        [Required]
        public int Work_Experience { get; set; }
        [Required]
        public int Movie_Number { get; set; }

        public List<Film> films { get; set; }
    }
}
