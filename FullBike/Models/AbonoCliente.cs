using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullBike.Models
{
    public class AbonoCliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int VentaId { get; set; }

        public DateTime FechaAbono { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal ValorAbono { get; set; }

        [StringLength(50)]
        public string MetodoPago { get; set; }

        [StringLength(100)]
        public string Comprobante { get; set; }

        [StringLength(200)]
        public string ObservacionesAbono { get; set; }

        // Relaciones
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("VentaId")]
        public virtual Venta Venta { get; set; }

        public AbonoCliente()
        {
            FechaAbono = DateTime.Now;
        }
    }
}