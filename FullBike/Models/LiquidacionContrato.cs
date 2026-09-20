using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullBike.Models
{
    public class LiquidacionContrato
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public string TipoSalida { get; set; }

        // Campos decimales - SIN configuraciones especiales
        public decimal SalarioPendiente { get; set; }
        public decimal VacacionesPendientes { get; set; }
        public decimal Cesantias { get; set; }
        public decimal InteresesCesantias { get; set; }
        public decimal PrimaServicios { get; set; }
        public decimal Indemnizacion { get; set; }
        public decimal TotalLiquidacion { get; set; }
        public decimal SaludDeducir { get; set; }
        public decimal PensionDeducir { get; set; }
        public decimal RetencionFuente { get; set; }
        public decimal NetoAPagar { get; set; }

        public string Observaciones { get; set; }

        [ForeignKey("EmpleadoId")]
        public virtual Empleado Empleado { get; set; }

        public LiquidacionContrato()
        {
            FechaLiquidacion = DateTime.Now;
            TipoSalida = "Renuncia";
        }
    }
}