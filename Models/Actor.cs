using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }

        public required string Biografia { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [ValidateNever]
        public ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
    }
}
