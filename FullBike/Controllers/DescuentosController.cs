using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using FullBike.Models;

namespace FullBike.Controllers
{
    public class DescuentosController : Controller
    {
        private ERPContext db = new ERPContext();

        // GET: Descuentos
        [Authorize]
        public ActionResult Index()
        {
            var descuentos = db.Descuentos
                .Include(d => d.Cliente)
                .Include(d => d.Categoria)
                .OrderByDescending(d => d.FechaCreacion)
                .ToList();
            return View(descuentos);
        }

        // GET: Descuentos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var descuento = db.Descuentos
                .Include(d => d.Cliente)
                .Include(d => d.Categoria)
                .FirstOrDefault(d => d.Id == id);
            if (descuento == null) return HttpNotFound();
            return View(descuento);
        }

        // GET: Descuentos/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Clientes = new SelectList(db.Clientes.Where(c => c.Activo), "Id", "NombreCompleto");
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Descuentos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Descuento descuento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    descuento.FechaCreacion = DateTime.Now;
                    descuento.CreadoPor = User.Identity.Name;
                    db.Descuentos.Add(descuento);
                    db.SaveChanges();
                    TempData["Mensaje"] = "Descuento guardado correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var err in errors)
                        System.Diagnostics.Debug.WriteLine(err.ErrorMessage);
                    TempData["Error"] = "No se pudo guardar el descuento. Revise los datos.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al guardar: " + ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            ViewBag.Clientes = new SelectList(db.Clientes.Where(c => c.Activo), "Id", "NombreCompleto", descuento.ClienteId);
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nombre", descuento.CategoriaId);
            return View(descuento);
        }

        // GET: Descuentos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var descuento = db.Descuentos.Find(id);
            if (descuento == null) return HttpNotFound();
            ViewBag.Clientes = new SelectList(db.Clientes.Where(c => c.Activo), "Id", "NombreCompleto", descuento.ClienteId);
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nombre", descuento.CategoriaId);
            return View(descuento);
        }

        // POST: Descuentos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Descuento descuento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(descuento).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Mensaje"] = "Descuento actualizado correctamente";
                    return RedirectToAction("Index");
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var err in errors)
                        System.Diagnostics.Debug.WriteLine(err.ErrorMessage);
                    TempData["Error"] = "No se pudo actualizar el descuento. Revise los datos.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar: " + ex.Message;
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            ViewBag.Clientes = new SelectList(db.Clientes.Where(c => c.Activo), "Id", "NombreCompleto", descuento.ClienteId);
            ViewBag.Categorias = new SelectList(db.Categorias, "Id", "Nombre", descuento.CategoriaId);
            return View(descuento);
        }

        // POST: Descuentos/ToggleActivo (para activar/desactivar desde Index)
        [HttpPost]
        public JsonResult ToggleActivo(int id)
        {
            var descuento = db.Descuentos.Find(id);
            if (descuento != null)
            {
                descuento.Activo = !descuento.Activo;
                db.Entry(descuento).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, activo = descuento.Activo });
            }
            return Json(new { success = false });
        }

        // GET: Descuentos/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            var descuento = db.Descuentos.Find(id);
            if (descuento == null) return HttpNotFound();
            return View(descuento);
        }

        // POST: Descuentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var descuento = db.Descuentos.Find(id);
            if (descuento != null)
            {
                db.Descuentos.Remove(descuento);
                db.SaveChanges();
                TempData["Mensaje"] = "Descuento eliminado";
            }
            return RedirectToAction("Index");
        }

        // GET: Descuentos/GetDescuentosDisponibles (para Ventas)
        public JsonResult GetDescuentosDisponibles(int clienteId, decimal subtotal)
        {
            var hoy = DateTime.Now;
            var descuentos = db.Descuentos
                .Where(d => d.Activo &&
                            d.FechaInicio <= hoy &&
                            (d.FechaFin == null || d.FechaFin >= hoy) &&
                            (d.MontoMinimo == null || subtotal >= d.MontoMinimo) &&
                            (d.AplicaTodosClientes || d.ClienteId == clienteId))
                .Select(d => new
                {
                    d.Id,
                    d.Nombre,
                    d.TipoDescuento,
                    d.Valor,
                    MontoAplicado = d.TipoDescuento == "Porcentaje"
                        ? Math.Round(subtotal * (d.Valor / 100), 2)
                        : d.Valor
                })
                .ToList();
            return Json(descuentos, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}