using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.Models
{
    public class PasswordRestore
    {
        [Required]
        public string Email { get; set; }
        public string RestoreCode { get; set; }
    }
}
