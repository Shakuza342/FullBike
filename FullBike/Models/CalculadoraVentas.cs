using System;
using System.Linq;

namespace FullBike.Models
{
    public static class CalculadoraVentas
    {
        // Constantes
        private const decimal IVA_PORCENTAJE = 0.19m;
        private const decimal RETENCION_PORCENTAJE = 0.025m; // 2.5% para montos > 1.000.000
        private const decimal DESCUENTO_MINIMO_VOLUMEN = 0.05m; // 5% para 5-9 unidades
        private const decimal DESCUENTO_MEDIO_VOLUMEN = 0.10m; // 10% para 10-19 unidades
        private const decimal DESCUENTO_MAXIMO_VOLUMEN = 0.15m; // 15% para 20+ unidades

        /// <summary>
        /// Calcular IVA (19%)
        /// </summary>
        public static decimal CalcularIVA(decimal subtotal)
        {
            return Math.Round(subtotal * IVA_PORCENTAJE, 2);
        }

        /// <summary>
        /// Calcular Retención en la Fuente
        /// </summary>
        public static decimal CalcularRetencion(decimal total)
        {
            return total > 1000000 ? Math.Round(total * RETENCION_PORCENTAJE, 2) : 0;
        }

        /// <summary>
        /// Calcular Descuento por volumen de compra
        /// </summary>
        public static decimal CalcularDescuentoPorVolumen(int cantidad, decimal precioUnitario)
        {
            decimal subtotal = cantidad * precioUnitario;

            if (cantidad >= 20)
                return Math.Round(subtotal * DESCUENTO_MAXIMO_VOLUMEN, 2);
            else if (cantidad >= 10)
                return Math.Round(subtotal * DESCUENTO_MEDIO_VOLUMEN, 2);
            else if (cantidad >= 5)
                return Math.Round(subtotal * DESCUENTO_MINIMO_VOLUMEN, 2);
            else
                return 0;
        }

        /// <summary>
        /// Calcular Saldo del Cliente (total ventas - total abonos)
        /// </summary>
        public static decimal CalcularSaldoCliente(ERPContext db, int clienteId)
        {
            var totalVentas = db.Ventas
                .Where(v => v.ClienteId == clienteId)
                .Sum(v => (decimal?)v.Total) ?? 0;

            var totalAbonos = db.AbonosClientes
                .Where(a => a.ClienteId == clienteId)
                .Sum(a => (decimal?)a.ValorAbono) ?? 0;

            return totalVentas - totalAbonos;
        }

        /// <summary>
        /// Calcular totales de una venta completa
        /// </summary>
        public static (decimal subtotal, decimal iva, decimal total) CalcularTotalesVenta(decimal subtotal)
        {
            var iva = CalcularIVA(subtotal);
            var total = subtotal + iva;
            return (subtotal, iva, total);
        }

        /// <summary>
        /// Calcular totales incluyendo descuentos
        /// </summary>
        public static (decimal subtotal, decimal descuentoTotal, decimal iva, decimal total) CalcularTotalesConDescuento(
            int[] cantidades, decimal[] preciosUnitarios)
        {
            decimal subtotal = 0;
            decimal descuentoTotal = 0;

            for (int i = 0; i < cantidades.Length; i++)
            {
                var descuento = CalcularDescuentoPorVolumen(cantidades[i], preciosUnitarios[i]);
                subtotal += cantidades[i] * preciosUnitarios[i];
                descuentoTotal += descuento;
            }

            var subtotalConDescuento = subtotal - descuentoTotal;
            var iva = CalcularIVA(subtotalConDescuento);
            var total = subtotalConDescuento + iva;

            return (subtotal, descuentoTotal, iva, total);
        }
    }
}