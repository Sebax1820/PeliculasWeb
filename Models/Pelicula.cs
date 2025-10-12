using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace PeliculasWeb.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public int Duracion { get; set; }
        public DateTime fechaEstreno { get; set; }
        public string imagenUrl { get; set; }
        public ICollection<Genero> Genero { get; set; }
        public ICollection<Director> Director { get; set; }
        public ICollection<Actor> Actores { get; set; }
    }
}
