
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullBike.Models
{
    public class DetalleCompra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CompraId { get; set; }

        [Required]
        public int RepuestoId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PrecioUnitario { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Total { get; set; }

        // Relaciones
        [ForeignKey("CompraId")]
        public virtual Compra Compra { get; set; }

        [ForeignKey("RepuestoId")]
        public virtual Repuesto Repuesto { get; set; }
    }
}