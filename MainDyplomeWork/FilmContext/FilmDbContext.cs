using Microsoft.EntityFrameworkCore;
using SmartReservationCinema.FilmContext;

namespace SmartReservationCinema.FilmContext
{
    public class FilmDbContext : DbContext
    {
        public FilmDbContext()
        {
           // Database.EnsureCreated();
        }
        public DbSet<Film> Films { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Genre_Film> GenresFilmes { get; set; }
        public DbSet<Director> Director { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Film_Actor> FilmActor { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Subtitle> Subtitles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FilmMark> FilmMarks { get; set; } 
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
            builder.Entity<Genre>().HasData(new Genre[]
            {
                new Genre()
                {
                    Id=1,
                    GenreName = "Action"
                },
                new Genre()
                {
                    Id=2,
                    GenreName = "Adventure"
                },
                new Genre()
                {
                    Id=3,
                    GenreName = "Comedy"
                },
                new Genre()
                {
                    Id=4,
                    GenreName = "Drama"
                },
                new Genre()
                {
                    Id=5,
                    GenreName = "Crime"
                },
                new Genre()
                {
                    Id = 6,
                    GenreName = "Horror"
                },
                new Genre()
                {
                    Id = 7,
                    GenreName = "Fantasy"
                },
                new Genre()
                {
                    Id = 8,
                    GenreName = "Romance"
                },
                new Genre()
                {
                    Id = 9,
                    GenreName = "Thriller"
                },
                new Genre()
                {
                    Id = 10,
                    GenreName = "Animation"
                },
                new Genre()
                {
                    Id = 11,
                    GenreName = "Family"
                },
                new Genre()
                {
                    Id = 12,
                    GenreName = "War"
                },
                new Genre()
                {
                    Id = 13,
                    GenreName = "Documentary"
                },
                new Genre()
                {
                    Id = 14,
                    GenreName = "Biography"
                },
                new Genre()
                {
                    Id = 15,
                    GenreName = "Sci-fi"
                },
                new Genre()
                {
                    Id = 16,
                    GenreName = "Western"
                },
                new Genre()
                {
                    Id= 17,
                    GenreName = "Post-apocalyptic"
                }
            });
            builder.Entity<Town>().HasData(
                new Town
                {
                    Id = 1,
                    TownName = "Warsawa",
                    Region = "Masovian Voivodeship"
                },
                new Town
                {
                    Id = 2,
                    TownName = "Krakow",
                    Region = "Lesser Poland Voivodeship"
                },
                new Town
                {
                    Id = 3,
                    TownName = "Zakopane",
                    Region = "Lesser Poland Voivodeship"
                },
                new Town
                {
                    Id = 4,
                    TownName = "Wroclaw",
                    Region = "Lower Silesian Voivodeship"
                },
                new Town
                {
                    Id = 5,
                    TownName = "Rzeszow",
                    Region = "Subcarpathian Voivodeship"
                },
                new Town
                {
                    Id = 6,
                    TownName = "Lodz",
                    Region = "Lodz Voivodeship"
                },
                new Town
                {
                    Id = 7,
                    TownName = "Poznan",
                    Region = "Greater Poland Voivodeship"
                },
                new Town
                {
                    Id = 8,
                    TownName = "Gdansk",
                    Region = "Pomeranian Voivodeship"
                },
                new Town
                {
                    Id = 9,
                    TownName = "Lublin",
                    Region = "Lublin Voivodeship"
                },
                new Town
                {
                    Id = 10,
                    TownName = "Bialystock",
                    Region = "Podlaskie Voivodeship"
                }
            );
        }
    }
}
