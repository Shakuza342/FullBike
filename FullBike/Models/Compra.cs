using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FullBike.Models
{
    public class Compra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaCompra { get; set; }

        [Required]
        public int ProveedorId { get; set; }

        [Required]
        public int EmpleadoId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Iva { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Total { get; set; }

        [StringLength(20)]
        public string Estado { get; set; } // Pendiente, Recibida, Cancelada

        [StringLength(50)]
        public string NumeroFactura { get; set; }

        [StringLength(500)]
        public string Observaciones { get; set; }

        // Relaciones
        public virtual Proveedor Proveedor { get; set; }
        public virtual Empleado Empleado { get; set; }
        public virtual ICollection<DetalleCompra> DetallesCompra { get; set; }

        public Compra()
        {
            FechaCompra = DateTime.Now;
            Estado = "Pendiente";
            DetallesCompra = new HashSet<DetalleCompra>();
        }
    }
}