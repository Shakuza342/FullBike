using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullBike.Models
{
    public class DetalleVenta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int VentaId { get; set; }

        [Required]
        public int RepuestoId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Descuento { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Total { get; set; }

        // Relaciones
        [ForeignKey("VentaId")]
        public virtual Venta Venta { get; set; }

        [ForeignKey("RepuestoId")]
        public virtual Repuesto Repuesto { get; set; }

        public DetalleVenta()
        {
            Descuento = 0;
        }

        // Propiedad calculada
        public decimal PrecioConDescuento => PrecioUnitario - Descuento;
    }
}