using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeliculasWeb.Models
{
    public class Pelicula
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        public string Titulo { get; set; }

        public string Sinopsis { get; set; }

        [Display(Name = "Duración")]
        public int Duracion { get; set; }

        [Display(Name = "Año de estreno")]
        public int AnioEstreno { get; set; }

        [Display(Name = "Portada")]
        public string imagenUrl { get; set; }

        [Display(Name = "Género")]
        public int GeneroId { get; set; }
        [ForeignKey("GeneroId")]
        public Genero Genero { get; set; }

        [Display(Name = "Director")]
        public int DirectorId { get; set; }
        [ForeignKey("DirectorId")]
        [ValidateNever]
        public Director Director { get; set; }

        public ICollection<Actor> Actores { get; set; }

    }
}
