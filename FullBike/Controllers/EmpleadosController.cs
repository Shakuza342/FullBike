using System;
using System.Linq;
using System.Web.Mvc;
using FullBike.Models;
using System.Data.Entity;

namespace FullBike.Controllers
{
    public class EmpleadosController : Controller
    {
        private ERPContext db = new ERPContext();

        // GET: Empleados
        public ActionResult Index()
        {
            var empleados = db.Empleados.Where(e => e.Activo).ToList();
            return View(empleados);
        }

        // GET: Empleados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                empleado.FechaCreacion = DateTime.Now;
                empleado.Activo = true;

                // Establecer valores por defecto para nuevos campos de nómina
                if (string.IsNullOrEmpty(empleado.Estado))
                    empleado.Estado = "Activo";

                if (string.IsNullOrEmpty(empleado.TipoContrato))
                    empleado.TipoContrato = "Indefinido";

                // Hash simple de contraseña (en producción usar BCrypt)
                if (!string.IsNullOrEmpty(empleado.PasswordHash))
                {
                    empleado.PasswordHash = SimpleHash(empleado.PasswordHash);
                }

                db.Empleados.Add(empleado);
                db.SaveChanges();
                TempData["Mensaje"] = "Empleado creado exitosamente";
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        public ActionResult Edit(int id)
        {
            var empleado = db.Empleados.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                // Mantener la contraseña existente si no se cambia
                var existingEmpleado = db.Empleados.AsNoTracking().FirstOrDefault(e => e.Id == empleado.Id);
                if (existingEmpleado != null)
                {
                    if (string.IsNullOrEmpty(empleado.PasswordHash))
                    {
                        empleado.PasswordHash = existingEmpleado.PasswordHash;
                    }
                    else
                    {
                        empleado.PasswordHash = SimpleHash(empleado.PasswordHash);
                    }

                    // Mantener fecha de creación original
                    empleado.FechaCreacion = existingEmpleado.FechaCreacion;
                }

                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensaje"] = "Empleado actualizado exitosamente";
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        public ActionResult Delete(int id)
        {
            var empleado = db.Empleados.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var empleado = db.Empleados.Find(id);
            if (empleado != null)
            {
                empleado.Activo = false;
                empleado.Estado = "Retirado";
                empleado.FechaTerminacion = DateTime.Now;
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensaje"] = "Empleado eliminado exitosamente";
            }
            return RedirectToAction("Index");
        }

        // Hash simple para desarrollo (NO usar en producción)
        private string SimpleHash(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
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