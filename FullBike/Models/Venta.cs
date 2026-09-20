using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FullBike.Models
{
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaVenta { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int EmpleadoId { get; set; }

        // Totales
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }

        // Estado de la venta
        [StringLength(20)]
        public string Estado { get; set; } // Pendiente, Completada, Cancelada

        // Forma de pago
        [StringLength(50)]
        public string MetodoPago { get; set; } // Efectivo, Tarjeta, Transferencia

        [StringLength(500)]
        public string Observaciones { get; set; }

        // Relaciones
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        [ForeignKey("EmpleadoId")]
        public virtual Empleado Empleado { get; set; }

        public virtual ICollection<DetalleVenta> DetallesVenta { get; set; }
        public virtual ICollection<Factura> Facturas { get; set; }
        public virtual ICollection<AbonoCliente> Abonos { get; set; }

        public Venta()
        {
            FechaVenta = DateTime.Now;
            Estado = "Pendiente";
            DetallesVenta = new HashSet<DetalleVenta>();
            Facturas = new HashSet<Factura>();
            Abonos = new HashSet<AbonoCliente>();
        }

        // Propiedades calculadas
        public decimal SaldoPendiente => Total - (Abonos?.Sum(a => a.ValorAbono) ?? 0);
        public bool EstaPagada => SaldoPendiente <= 0;
        public string EstadoPago => EstaPagada ? "Pagada" : $"Debe: {SaldoPendiente:C}";
    }
}