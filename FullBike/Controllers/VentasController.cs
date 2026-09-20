using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using FullBike.Models;

namespace FullBike.Controllers
{
    public class VentasController : Controller
    {
        private ERPContext db = new ERPContext();

        [Authorize]
        public ActionResult Index()
        {
            var ventas = db.Ventas
                .Include("Cliente")
                .Include("Empleado")
                .OrderByDescending(v => v.FechaVenta)
                .ToList();
            return View(ventas);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var venta = db.Ventas
                .Include("Cliente")
                .Include("Empleado")
                .Include("DetallesVenta.Repuesto")
                .Include("Facturas")
                .Include("Abonos")
                .FirstOrDefault(v => v.Id == id);
            if (venta == null) return HttpNotFound();
            return View(venta);
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Clientes = new SelectList(db.Clientes.Where(c => c.Activo), "Id", "NombreCompleto");
            ViewBag.Repuestos = db.Repuestos.Where(r => r.Activo && r.Stock > 0).ToList();
            ViewBag.Empleados = new SelectList(db.Empleados.Where(e => e.Activo), "Id", "NombreCompleto");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int clienteId, int empleadoId, int[] repuestoIds, int[] cantidades,
                                    decimal[] precios, int? descuentoId, string metodoPago, string observaciones)
        {
            try
            {
                if (repuestoIds == null || repuestoIds.Length == 0 || repuestoIds[0] == 0)
                    throw new Exception("Debe seleccionar al menos un producto.");

                // Validar que existan los arrays de cantidades y precios
                if (cantidades == null || precios == null || cantidades.Length != repuestoIds.Length || precios.Length != repuestoIds.Length)
                    throw new Exception("Datos de productos incompletos.");

                using (var transaction = db.Database.BeginTransaction())
                {
                    decimal subtotal = 0;
                    for (int i = 0; i < repuestoIds.Length; i++)
                    {
                        if (repuestoIds[i] > 0 && cantidades[i] > 0)
                        {
                            decimal precioUnitario = precios[i];

                            // 🔧 Si el precio enviado es 0, tomar el precio real de la base de datos
                            if (precioUnitario <= 0)
                            {
                                var producto = db.Repuestos.Find(repuestoIds[i]);
                                if (producto != null && producto.Precio > 0)
                                    precioUnitario = producto.Precio;
                                else
                                    throw new Exception($"El producto con ID {repuestoIds[i]} no tiene un precio válido en la base de datos.");
                                // Actualizar el array para guardar el valor correcto en DetalleVenta
                                precios[i] = precioUnitario;
                            }
                            subtotal += cantidades[i] * precioUnitario;
                        }
                    }

                    decimal valorDescuento = 0;
                    if (descuentoId.HasValue && descuentoId > 0)
                    {
                        var d = db.Descuentos.Find(descuentoId);
                        if (d != null && d.Activo && (d.MontoMinimo == null || subtotal >= d.MontoMinimo))
                            valorDescuento = d.TipoDescuento == "Porcentaje" ? subtotal * (d.Valor / 100) : d.Valor;
                    }

                    decimal iva = subtotal * 0.19m;
                    decimal totalConDescuento = subtotal + iva - valorDescuento;
                    decimal retencion = totalConDescuento > 5000000 ? totalConDescuento * 0.035m : (totalConDescuento > 1000000 ? totalConDescuento * 0.025m : 0);
                    decimal total = totalConDescuento - retencion;

                    var venta = new Venta
                    {
                        FechaVenta = DateTime.Now,
                        ClienteId = clienteId,
                        EmpleadoId = empleadoId,
                        Subtotal = subtotal,
                        Iva = iva,
                        Descuento = valorDescuento,
                        Total = total,
                        Estado = "Completada",
                        MetodoPago = metodoPago ?? "Efectivo",
                        Observaciones = observaciones ?? ""
                    };
                    db.Ventas.Add(venta);
                    db.SaveChanges();

                    for (int i = 0; i < repuestoIds.Length; i++)
                    {
                        if (repuestoIds[i] > 0 && cantidades[i] > 0)
                        {
                            var detalle = new DetalleVenta
                            {
                                VentaId = venta.Id,
                                RepuestoId = repuestoIds[i],
                                Cantidad = cantidades[i],
                                PrecioUnitario = precios[i],
                                Descuento = 0,
                                Total = cantidades[i] * precios[i]
                            };
                            db.DetallesVenta.Add(detalle);
                            var prod = db.Repuestos.Find(repuestoIds[i]);
                            if (prod != null) prod.Stock -= cantidades[i];
                        }
                    }
                    db.SaveChanges();

                    if (descuentoId.HasValue && valorDescuento > 0)
                    {
                        db.DescuentosAplicados.Add(new DescuentoAplicado
                        {
                            VentaId = venta.Id,
                            DescuentoId = descuentoId.Value,
                            ValorAplicado = valorDescuento,
                            FechaAplicacion = DateTime.Now
                        });
                        db.SaveChanges();
                    }

                    db.Facturas.Add(new Factura
                    {
                        VentaId = venta.Id,
                        NumeroFactura = $"FAC-{venta.Id}-{DateTime.Now.Year}",
                        Subtotal = subtotal,
                        IvaTotal = iva,
                        DescuentoTotal = valorDescuento,
                        RetencionTotal = retencion,
                        TotalPagado = total,
                        EstadoFactura = "Pagada",
                        FechaEmision = DateTime.Now
                    });
                    db.SaveChanges();

                    transaction.Commit();
                    TempData["Mensaje"] = $"Venta #{venta.Id} creada. Total: {total:C}";
                    return RedirectToAction("Details", new { id = venta.Id });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return RedirectToAction("Create");
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var venta = db.Ventas.Find(id);
            if (venta == null) return HttpNotFound();
            return View(venta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var venta = db.Ventas.Find(id);
                if (venta != null)
                {
                    var detalles = db.DetallesVenta.Where(d => d.VentaId == id);
                    var facturas = db.Facturas.Where(f => f.VentaId == id);
                    var abonos = db.AbonosClientes.Where(a => a.VentaId == id);
                    var descuentosAplicados = db.DescuentosAplicados.Where(da => da.VentaId == id);
                    db.DetallesVenta.RemoveRange(detalles);
                    db.Facturas.RemoveRange(facturas);
                    db.AbonosClientes.RemoveRange(abonos);
                    db.DescuentosAplicados.RemoveRange(descuentosAplicados);
                    db.Ventas.Remove(venta);
                    db.SaveChanges();
                    TempData["Mensaje"] = "Venta eliminada correctamente";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}