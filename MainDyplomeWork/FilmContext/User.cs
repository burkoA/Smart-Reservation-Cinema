using MainDyplomeWork.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace MainDyplomeWork.FilmContext
{
    [Index("Email",IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
        public const string AdminRole = "admin";
        public const string UserRole = "user";
        public const string ManagerRole = "manager";
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; } = "";
        public string City { get; set; }
        public int Age { get; set; }
        public DateTime RegisterDate { get; set; }
        public User()
        {

        }
        public User(RegistrationModel model)
        {
            Email = model.Email;
            Password = GetPasswordHash(model.Password);
            FirstName = model.FirstName;
            LastName = model.LastName;
            City = model.City;
            Role = UserRole;
        }
        public static string GetPasswordHash(String password)
        {
            return password;
        }
    }
}
