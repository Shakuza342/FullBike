using System;
using System.ComponentModel.DataAnnotations;

namespace FullBike.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(1000)]
        public string Descripcion { get; set; }

        [StringLength(50)]
        public string Estado { get; set; } = "Pendiente";

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}