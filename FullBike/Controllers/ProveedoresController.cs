using System;
using System.Linq;
using System.Web.Mvc;
using FullBike.Models;
using System.Data.Entity;

namespace FullBike.Controllers
{
    public class ProveedoresController : Controller
    {
        private ERPContext db = new ERPContext();

        // GET: Proveedores
        public ActionResult Index()
        {
            var proveedores = db.Proveedores.Where(p => p.Activo).ToList();
            return View(proveedores);
        }

        // GET: Proveedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                proveedor.FechaRegistro = DateTime.Now;
                proveedor.Activo = true;
                db.Proveedores.Add(proveedor);
                db.SaveChanges();
                TempData["Mensaje"] = "Proveedor creado exitosamente";
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        // GET: Proveedores/Edit/5
        public ActionResult Edit(int id)
        {
            var proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proveedor).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensaje"] = "Proveedor actualizado exitosamente";
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5
        public ActionResult Delete(int id)
        {
            var proveedor = db.Proveedores.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var proveedor = db.Proveedores.Find(id);
            if (proveedor != null)
            {
                proveedor.Activo = false;
                db.Entry(proveedor).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensaje"] = "Proveedor eliminado exitosamente";
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