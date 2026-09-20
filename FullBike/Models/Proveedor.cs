using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FullBike.Models
{
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La razón social es requerida")]
        [StringLength(150)]
        public string RazonSocial { get; set; }

        [Required]
        [StringLength(100)]
        public string Contacto { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Nit { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Relación con Repuestos
        public virtual ICollection<Repuesto> Repuestos { get; set; }

        public Proveedor()
        {
            FechaRegistro = DateTime.Now;
            Activo = true;
            Repuestos = new HashSet<Repuesto>();
        }
    }
}