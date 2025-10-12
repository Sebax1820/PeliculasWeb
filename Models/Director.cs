using System.ComponentModel.DataAnnotations;

namespace PeliculasWeb.Models
{
    public class Director
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Nacionalidad { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        public ICollection<Pelicula> Peliculas { get; set; }
    }
}
