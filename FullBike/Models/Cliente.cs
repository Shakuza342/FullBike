using System;
using System.ComponentModel.DataAnnotations;

namespace FullBike.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

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

        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        [StringLength(50)]
        public string TipoCliente { get; set; } // Regular, Premium, Corporativo

        public Cliente()
        {
            FechaRegistro = DateTime.Now;
            Activo = true;
            TipoCliente = "Regular";
        }

        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}