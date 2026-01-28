using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace yotelollevo.Controllers
{
    [RoleAuthorize("ADMIN")]
    public class UsuariosController : Controller
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();

        public ActionResult Index()
        {
            var usuarios = db.Usuario
                .Select(u => new
                {
                    u.IdUsuario,
                    u.Nombre,
                    u.Login,
                    Rol = u.Rol.NombreRol,
                    u.Activo
                })
                .ToList();

            return View(usuarios);
        }
    }
}
