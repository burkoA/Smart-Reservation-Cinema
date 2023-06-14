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
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer("Server=LAPTOP-84PUUS9F\\SQLEXPRESS;Database=FilmsData;Trusted_Connection=True");
        }
        public DbSet<MainDyplomeWork.FilmContext.Director> Director { get; set; }
    }
}
