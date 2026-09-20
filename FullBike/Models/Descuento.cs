using System;
using System.ComponentModel.DataAnnotations;

namespace FullBike.Models
{
    public class Descuento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del descuento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el tipo de descuento (Porcentaje o MontoFijo).")]
        public string TipoDescuento { get; set; }

        [Required(ErrorMessage = "El valor del descuento es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor del descuento debe ser mayor a 0.")]
        public decimal Valor { get; set; }

        public bool AplicaTodosClientes { get; set; } = true;
        public int? ClienteId { get; set; }

        public bool AplicaTodosProductos { get; set; } = true;
        public int? CategoriaId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El monto mínimo no puede ser negativo.")]
        public decimal? MontoMinimo { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaFin { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public string CreadoPor { get; set; }

        // Relaciones
        public virtual Cliente Cliente { get; set; }
        public virtual Categoria Categoria { get; set; }
    }
}