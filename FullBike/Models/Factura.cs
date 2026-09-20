using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullBike.Models
{
    public class Factura
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int VentaId { get; set; }

        [StringLength(50)]
        public string NumeroFactura { get; set; }

        public DateTime FechaEmision { get; set; }

        // Desglose de la factura
        public decimal Subtotal { get; set; }
        public decimal IvaTotal { get; set; }
        public decimal DescuentoTotal { get; set; }
        public decimal RetencionTotal { get; set; }
        public decimal TotalPagado { get; set; }

        // Información de la factura
        [StringLength(50)]
        public string EstadoFactura { get; set; } // Generada, Pagada, Anulada

        [StringLength(500)]
        public string ObservacionesFactura { get; set; }

        // Datos de facturación (para imprimir)
        [StringLength(100)]
        public string NombreClienteFactura { get; set; }

        [StringLength(50)]
        public string NitCliente { get; set; }

        [StringLength(200)]
        public string DireccionCliente { get; set; }

        // Relaciones
        [ForeignKey("VentaId")]
        public virtual Venta Venta { get; set; }

        public Factura()
        {
            FechaEmision = DateTime.Now;
            EstadoFactura = "Generada";
        }

        // Propiedades calculadas
        public string NumeroFacturaFormateado => $"FAC-{Id:D8}-{FechaEmision:yyyy}";
    }
}