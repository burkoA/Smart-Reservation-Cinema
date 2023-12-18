using SmartReservationCinema.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartReservationCinema.FilmContext
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
        [NotMapped]
        public bool IsAdminSelected { get; set; }
        [NotMapped]
        public bool IsUserSelected { get; set; }
        [NotMapped]
        public bool IsManagerSelected { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; } = "";
        //[Required]
        public string City { get; set; }
        //[Required]
        public string Address { get; set; }
        public int? Age { get; set; }
        public DateTime RegisterDate { get; set; }
        //public int RestoreCode { get; set; }
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
            Address = model.Address;
            Role = UserRole;
            RegisterDate = DateTime.Now;
        }
        public void Update(ProfileModel profile)
        {
            FirstName = profile.FirstName;
            LastName = profile.LastName;
            City = profile.City;
            Address = profile.Address;
            Age = profile.Age;
        }
        public static string GetPasswordHash(String password)
        {
            SHA512 sha512 = SHA512.Create();
            return Convert.ToBase64String(sha512.ComputeHash(Encoding.UTF8.GetBytes(password)));            
        }
    }
}
