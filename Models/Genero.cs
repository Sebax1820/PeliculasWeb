

namespace PeliculasWeb.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }

        public ICollection<Pelicula>? Peliculas { get; set; }
    }
}
