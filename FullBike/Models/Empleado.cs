using System;
using System.ComponentModel.DataAnnotations;

namespace FullBike.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        // Información básica (existente)
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(20)]
        public string Documento { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        // Información laboral (ampliada)
        [Required]
        [StringLength(50)]
        public string Cargo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaContratacion { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaTerminacion { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SalarioBase { get; set; } // ⚠️ QUITAR Column(TypeName = "decimal(18,2)")

        // Tipo de contrato
        [Required]
        [StringLength(20)]
        public string TipoContrato { get; set; } // Indefinido, Fijo, ObraLabor, Aprendizaje

        // Estado del empleado
        [Required]
        [StringLength(20)]
        public string Estado { get; set; } // Activo, Vacaciones, Incapacitado, Retirado

        // Información bancaria
        [StringLength(50)]
        public string Banco { get; set; }

        [StringLength(20)]
        public string NumeroCuenta { get; set; }

        [StringLength(10)]
        public string TipoCuenta { get; set; } // Ahorros, Corriente

        // Información de seguridad social
        [StringLength(20)]
        public string EPS { get; set; }

        [StringLength(20)]
        public string Pension { get; set; }

        [StringLength(20)]
        public string ARL { get; set; }

        [StringLength(20)]
        public string CajaCompensacion { get; set; }

        // Campos existentes
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        [StringLength(100)]
        public string Usuario { get; set; }

        [StringLength(255)]
        public string PasswordHash { get; set; }

        [StringLength(50)]
        public string Rol { get; set; }

        public Empleado()
        {
            FechaCreacion = DateTime.Now;
            Activo = true;
            Rol = "Empleado";
            Estado = "Activo";
            TipoContrato = "Indefinido";
        }

        public string NombreCompleto => $"{Nombre} {Apellido}";

        // Propiedad calculada - Días trabajados
        public int DiasTrabajados
        {
            get
            {
                var fechaFin = FechaTerminacion ?? DateTime.Now;
                return (fechaFin - FechaContratacion).Days;
            }
        }

        // Propiedad calculada - Meses trabajados
        public int MesesTrabajados
        {
            get
            {
                var fechaFin = FechaTerminacion ?? DateTime.Now;
                return ((fechaFin.Year - FechaContratacion.Year) * 12) + fechaFin.Month - FechaContratacion.Month;
            }
        }
    }
}