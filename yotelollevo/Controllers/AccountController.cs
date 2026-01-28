using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yotelollevo.Controllers
{
    public class AccountController : Controller
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string login, string clave)
        {
            var user = db.Usuario
                .Where(u => u.Login == login && u.ClaveHash == clave && u.Activo == true)
                .Select(u => new
                {
                    u.IdUsuario,
                    u.Nombre,
                    u.IdRol,
                    u.IdTienda,
                    u.IdRepartidor
                })
                .FirstOrDefault();

            if (user == null)
            {
                ViewBag.Error = "Usuario o clave incorrectos (o inactivo).";
                return View();
            }

            var rolNombre = db.Rol
                .Where(r => r.IdRol == user.IdRol)
                .Select(r => r.NombreRol)   // ojo: en tu tabla se llama NombreRol
                .FirstOrDefault();

            Session["UserId"] = user.IdUsuario;
            Session["UserName"] = user.Nombre;
            Session["Rol"] = rolNombre;
            Session["IdTienda"] = user.IdTienda;
            Session["IdRepartidor"] = user.IdRepartidor;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
