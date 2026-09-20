using System;
using System.Linq;
using System.Web.Mvc;
using FullBike.Models;
using System.Data.Entity;

namespace FullBike.Controllers
{
    public class RequestosController : Controller
    {
        private ERPContext db = new ERPContext();

        // GET: Requestos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Requestos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Request request)
        {
            if (ModelState.IsValid)
            {
                request.FechaCreacion = DateTime.Now;
                request.Estado = "Pendiente";
                db.Requests.Add(request);
                db.SaveChanges();
                TempData["Mensaje"] = "Solicitud creada exitosamente";
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // GET: Requestos/Index
        public ActionResult Index()
        {
            var requests = db.Requests.ToList();
            return View(requests);
        }
    }
}