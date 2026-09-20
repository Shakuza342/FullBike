using System;
using System.Linq;
using System.Web.Mvc;
using FullBike.Models;
using System.Data.Entity;

namespace FullBike.Controllers
{
    public class ClientesController : Controller
    {
        private ERPContext db = new ERPContext();

        // GET: Clientes
        public ActionResult Index()
        {
            var clientes = db.Clientes.Where(c => c.Activo).ToList();
            return View(clientes);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.FechaRegistro = DateTime.Now;
                cliente.Activo = true;
                db.Clientes.Add(cliente);
                db.SaveChanges();
                TempData["Mensaje"] = "Cliente creado exitosamente";
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int id)
        {
            var cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensaje"] = "Cliente actualizado exitosamente";
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int id)
        {
            var cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var cliente = db.Clientes.Find(id);
            if (cliente != null)
            {
                cliente.Activo = false;
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensaje"] = "Cliente eliminado exitosamente";
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