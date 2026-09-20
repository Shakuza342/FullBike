using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullBike.Models
{
    public class DescuentoAplicado
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int DescuentoId { get; set; }
        public decimal ValorAplicado { get; set; }
        public string TipoAplicado { get; set; }
        public DateTime FechaAplicacion { get; set; } = DateTime.Now;

        [ForeignKey("VentaId")] public virtual Venta Venta { get; set; }
        [ForeignKey("DescuentoId")] public virtual Descuento Descuento { get; set; }
    }
}