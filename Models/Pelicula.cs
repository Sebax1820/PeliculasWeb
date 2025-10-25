using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeliculasWeb.Models
{
    public class Pelicula
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        public required string Titulo { get; set; }

        public required string Sinopsis { get; set; }

        [Display(Name = "Duración")]
        public int Duracion { get; set; }

        [Display(Name = "Año de estreno")]
        public int AnioEstreno { get; set; }

        [Display(Name = "Género")]
        public int GeneroId { get; set; }
        [ForeignKey("GeneroId")]
        [ValidateNever]
        public required Genero Genero { get; set; }

        [Display(Name = "Director")]
        public int DirectorId { get; set; }
        [ForeignKey("DirectorId")]
        [ValidateNever]
        public required Director Director { get; set; }

        [ValidateNever]
        public required ICollection<Actor> Actores { get; set; } = new List<Actor>();

        [Display(Name = "Portada")]
        public string? ImagenRuta { get; set; }
        [NotMapped]
        public IFormFile? ImagenArchivo { get; set; }

    }
}
