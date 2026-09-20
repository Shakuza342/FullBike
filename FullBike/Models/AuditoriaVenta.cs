using System;
using System.ComponentModel.DataAnnotations;

namespace FullBike.Models
{
    public class AuditoriaVenta
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string TablaAfectada { get; set; }

        [StringLength(20)]
        public string Accion { get; set; }

        public int RegistroId { get; set; }

        [StringLength(100)]
        public string Usuario { get; set; }

        public DateTime FechaHora { get; set; } = DateTime.Now;

        public string DatosAnteriores { get; set; }

        public string DatosNuevos { get; set; }
    }
}