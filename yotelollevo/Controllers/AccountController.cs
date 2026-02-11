using System.Linq;
using System.Web.Mvc;
using yotelollevo.Constants;
using yotelollevo.Infrastructure;
using yotelollevo.Security;

namespace yotelollevo.Controllers
{
    public class AccountController : BaseController
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            if (!string.IsNullOrEmpty(CurrentUser.Rol))
            {
                if (CurrentUser.IsRepartidor)
                    return RedirectToAction("Inicio", "Rutas");

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string login, string clave)
        {
            const string msgError = "Usuario o contrasena incorrectos.";

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(clave))
            {
                ViewBag.Error = "Debes ingresar usuario y contrasena.";
                return View();
            }

            login = login.Trim();

            if (LoginThrottle.IsThrottled(login))
            {
                ViewBag.Error = "Demasiados intentos fallidos. Intenta nuevamente en 15 minutos.";
                return View();
            }

            var user = db.Usuario
                .Where(u => u.Login == login && u.Activo == true)
                .Select(u => new
                {
                    u.IdUsuario,
                    u.Nombre,
                    u.IdRol,
                    u.IdTienda,
                    u.IdRepartidor,
                    u.PasswordHash,
                    u.PasswordSalt
                })
                .FirstOrDefault();

            if (user == null)
            {
                LoginThrottle.RecordFailure(login);
                ViewBag.Error = msgError;
                return View();
            }

            if (user.PasswordHash == null || user.PasswordSalt == null)
            {
                ViewBag.Error = "Tu usuario no esta migrado a contrasena segura. Contacta al administrador.";
                return View();
            }

            bool ok = PasswordHasher.Verify(clave, user.PasswordHash, user.PasswordSalt);
            if (!ok)
            {
                LoginThrottle.RecordFailure(login);
                ViewBag.Error = msgError;
                return View();
            }

            var rolNombre = db.Rol
                .Where(r => r.IdRol == user.IdRol)
                .Select(r => r.NombreRol)
                .FirstOrDefault();

            rolNombre = (rolNombre ?? "").Trim();
            var rolUpper = rolNombre.ToUpperInvariant();

            if (string.IsNullOrEmpty(rolNombre))
            {
                ViewBag.Error = "Tu usuario no tiene un rol valido asignado. Contacta al administrador.";
                return View();
            }

            if (rolUpper == RoleNames.Repartidor)
            {
                if (user.IdRepartidor == null || user.IdRepartidor <= 0)
                {
                    ViewBag.Error = "Tu cuenta no esta vinculada a un repartidor valido. Contacta al administrador.";
                    return View();
                }

                int idRep = user.IdRepartidor.Value;
                bool repActivo = db.Repartidor.Any(r => r.IdRepartidor == idRep && r.Activo == true);

                if (!repActivo)
                {
                    ViewBag.Error = "Tu repartidor esta inactivo o no existe. Contacta al administrador.";
                    return View();
                }
            }

            LoginThrottle.Reset(login);
            HttpUserSession.SetLogin(Session, user.IdUsuario, user.Nombre, rolUpper, user.IdTienda, user.IdRepartidor);

            if (rolUpper == RoleNames.Repartidor)
                return RedirectToAction("Inicio", "Rutas");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
