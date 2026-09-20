using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using FullBike.Models;

public class HomeController : Controller
{
    private ERPContext db = new ERPContext();

    // GET: Home/Login
    public ActionResult Login()
    {
        return View();
    }

    // POST: Home/Login
    [HttpPost]
    public ActionResult Login(string usuario, string contraseña)
    {
        if (usuario == "admin" && contraseña == "admin123")
        {
            FormsAuthentication.SetAuthCookie(usuario, false);
            return RedirectToAction("Dashboard");
        }

        ViewBag.Error = "Usuario o contraseña incorrectos";
        return View();
    }

    // GET: Home/Dashboard
    [Authorize]
    public ActionResult Dashboard()
    {
        // Obtener estadísticas REALES de la base de datos
        var totalProductos = db.Repuestos.Count(r => r.Activo);
        var stockBajo = db.Repuestos.Count(r => r.Stock < 10 && r.Stock > 0 && r.Activo);
        var stockAgotado = db.Repuestos.Count(r => r.Stock == 0 && r.Activo);

        // 👇 SOLUCIÓN DEL ERROR: Usar null coalescing (??)
        var valorInventario = db.Repuestos.Where(r => r.Activo).Sum(r => (decimal?)r.Precio * r.Stock) ?? 0;

        // Pasar datos a la vista
        ViewBag.TotalProductos = totalProductos;
        ViewBag.StockBajo = stockBajo;
        ViewBag.StockAgotado = stockAgotado;
        ViewBag.ValorInventario = valorInventario.ToString("C0");

        return View();
    }

    // GET: Home/Index
    public ActionResult Index()
    {
        return View();
    }

    // POST: Home/Logout (Cerrar sesión)
    public ActionResult Logout()
    {
        FormsAuthentication.SignOut();
        return RedirectToAction("Index");
    }
}