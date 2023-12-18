using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.Models
{
    public class EnterNewPassword
    {
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords are different!")]
        [Required]
        public string RepeatPassword { get; set; }
    }
}
