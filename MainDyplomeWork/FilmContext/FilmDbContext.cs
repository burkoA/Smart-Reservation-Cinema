using Microsoft.EntityFrameworkCore;
using MainDyplomeWork.FilmContext;

namespace MainDyplomeWork.FilmContext
{
    public class FilmDbContext : DbContext
    {
        public FilmDbContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Film> Films { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Genre_Film> GenresFilmes { get; set; }
        public DbSet<MainDyplomeWork.FilmContext.Director> Director { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer("Server=LAPTOP-84PUUS9F\\SQLEXPRESS;Database=FilmsData;Trusted_Connection=True");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasData(new User[]
                {
                    new User()
                    {
                        Id = 1,
                        Email = "admin@gmail.com",
                        Password = "12345",
                        Role = User.AdminRole,
                        FirstName = "Arsen"
                    },
                    new User()
                    {
                        Id = 2,
                        Email = "manager@gmail.com",
                        Password = "123",
                        Role = User.ManagerRole,
                        FirstName = "Andrzej"
                    },
                    new User()
                    {
                        Id = 3,
                        Email = "user@gmail.com",
                        Password = "321",
                        Role = User.UserRole,
                        FirstName = "Kuba"
                    }
                });
            //builder.Entity<Genre>().HasData(new Genre[]
            //{

            //});
        }
    }
}
