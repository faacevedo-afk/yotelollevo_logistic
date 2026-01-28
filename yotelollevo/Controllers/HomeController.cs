using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace yotelollevo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new LogisticaDBEntities())
            {
                ViewBag.TotalTiendas = db.Tienda.Count();
                ViewBag.TotalPaquetes = db.Paquete.Count();
                ViewBag.TotalRutas = db.Ruta.Count();
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}


