using System.Linq;
using System.Web.Mvc;
using yotelollevo.Constants;
using yotelollevo.Filter;

namespace yotelollevo.Controllers
{
    [RoleAuthorize(RoleNames.Admin)]
    public class UsuariosController : BaseController
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

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
