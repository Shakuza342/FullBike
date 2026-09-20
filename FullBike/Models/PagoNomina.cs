using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullBike.Models
{
    public class PagoNomina
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmpleadoId { get; set; }

        [Required]
        public DateTime PeriodoInicio { get; set; }

        [Required]
        public DateTime PeriodoFin { get; set; }

        [Required]
        public DateTime FechaPago { get; set; }

        // Devengados - SIN Column Type
        [Required]
        public decimal SalarioBasico { get; set; }

        public decimal HorasExtras { get; set; }

        public decimal Bonificaciones { get; set; }

        public decimal Comisiones { get; set; }

        public decimal TotalDevengado { get; set; }

        // Deducciones - SIN Column Type
        public decimal SaludEmpleado { get; set; }

        public decimal PensionEmpleado { get; set; }

        public decimal RetencionFuente { get; set; }

        public decimal Prestamos { get; set; }

        public decimal OtrasDeducciones { get; set; }

        public decimal TotalDeducciones { get; set; }

        public decimal NetoAPagar { get; set; }

        // Aportes empresa - SIN Column Type
        public decimal SaludEmpresa { get; set; }

        public decimal PensionEmpresa { get; set; }

        public decimal ARLEmpresa { get; set; }

        public decimal CajaCompensacion { get; set; }

        public decimal ICBF { get; set; }

        public decimal SENA { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        // Relaciones
        [ForeignKey("EmpleadoId")]
        public virtual Empleado Empleado { get; set; }

        public PagoNomina()
        {
            FechaPago = DateTime.Now;
            PeriodoInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            PeriodoFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                    DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            Estado = "Pendiente";
        }
    }
}