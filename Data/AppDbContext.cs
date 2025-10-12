using Microsoft.EntityFrameworkCore;
using PeliculasWeb.Models;

namespace PeliculasWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Director> Directores { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
    }
    
}
