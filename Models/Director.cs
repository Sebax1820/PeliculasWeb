using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class Director
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Nacionalidad { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        public ICollection<Pelicula>? Peliculas { get; set; }
    }
}
