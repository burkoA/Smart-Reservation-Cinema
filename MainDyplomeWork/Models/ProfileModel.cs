using SmartReservationCinema.FilmContext;
using System;
using System.ComponentModel.DataAnnotations;

namespace SmartReservationCinema.Models
{
    public class ProfileModel
    {
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; } = "";
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        public int? Age { get; set; }
        public DateTime RegisterDate { get; set; }
        public ProfileModel()
        {

        }
        public ProfileModel(User user)
        {
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            City = user.City;
            Address = user.Address;
            Age = user.Age;
            RegisterDate = user.RegisterDate;
        }
    }
}
