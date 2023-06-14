using System.ComponentModel.DataAnnotations;

namespace MainDyplomeWork.FilmContext
{
    public class Actor
    {
        [Key]
        public int Id_Actor { get; set; }
        [Required]
        public string Actor_Name { get; set; } = "";
        [Required]
        public string Nationality { get; set; } = "";
    }
}
