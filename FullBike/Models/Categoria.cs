using System.ComponentModel.DataAnnotations;

namespace FullBike.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required] public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; } = true;
    }
}