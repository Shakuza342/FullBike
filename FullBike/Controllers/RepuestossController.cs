using System;
using System.Linq;
using System.Web.Mvc;
using FullBike.Models;
using System.Data.Entity;

namespace FullBike.Controllers
{
    public class RepuestosController : Controller
    {
        private ERPContext db = new ERPContext();

        // GET: Repuestos - Lista todos los productos
        public ActionResult Index()
        {
            var repuestos = db.Repuestos.Where(r => r.Activo).ToList();
            return View(repuestos);
        }

        // GET: Repuestos/Create - Formulario crear producto
        public ActionResult Create()
        {
            return View();
        }

        // POST: Repuestos/Create - Crear nuevo producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                repuesto.FechaCreacion = DateTime.Now;
                repuesto.Activo = true;
                db.Repuestos.Add(repuesto);
                db.SaveChanges();
                TempData["Mensaje"] = "Producto creado exitosamente";
                return RedirectToAction("Index");
            }
            return View(repuesto);
        }

        // GET: Repuestos/Edit/5 - Formulario editar producto
        public ActionResult Edit(int id)
        {
            var repuesto = db.Repuestos.Find(id);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            return View(repuesto);
        }

        // POST: Repuestos/Edit/5 - Actualizar producto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                repuesto.FechaActualizacion = DateTime.Now;
                db.Entry(repuesto).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensaje"] = "Producto actualizado exitosamente";
                return RedirectToAction("Index");
            }
            return View(repuesto);
        }

        // GET: Repuestos/Delete/5 - Confirmar eliminación
        public ActionResult Delete(int id)
        {
            var repuesto = db.Repuestos.Find(id);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            return View(repuesto);
        }

        // POST: Repuestos/Delete/5 - Eliminar producto
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var repuesto = db.Repuestos.Find(id);
            if (repuesto != null)
            {
                // Eliminación suave (solo marcar como inactivo)
                repuesto.Activo = false;
                repuesto.FechaActualizacion = DateTime.Now;
                db.Entry(repuesto).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensaje"] = "Producto eliminado exitosamente";
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}