using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using yotelollevo.Constants;
using yotelollevo.Filter;
using yotelollevo.Services;

namespace yotelollevo.Controllers
{
    public class RutasController : BaseController
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();
        private readonly IRutaService _rutaService;
        private readonly IEstadoService _estadoService;
        private readonly IDropdownService _dropdownService;

        public RutasController()
        {
            var estadoService = new EstadoService(db);
            _estadoService = estadoService;
            _rutaService = new RutaService(db, estadoService);
            _dropdownService = new DropdownService(db);
        }

        [RoleAuthorize(RoleNames.Admin)]
        public ActionResult Index()
        {
            return View(_rutaService.GetAll());
        }

        [RoleAuthorize(RoleNames.Admin, RoleNames.Repartidor)]
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ruta = _rutaService.GetByIdWithDetails(id.Value);
            if (ruta == null) return HttpNotFound();

            if (CurrentUser.IsRepartidor)
            {
                int idRep = CurrentUser.IdRepartidor ?? 0;
                if (idRep <= 0) return RedirectToAction("Login", "Account");
                if (ruta.IdRepartidor != idRep) return new HttpStatusCodeResult(403);
            }

            return View(ruta);
        }

        [RoleAuthorize(RoleNames.Admin)]
        public ActionResult Create()
        {
            CargarCombos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(RoleNames.Admin)]
        public ActionResult Create([Bind(Include = "IdRuta,FechaRuta,IdRepartidor")] Ruta ruta)
        {
            if (!_rutaService.IsRepartidorActivo(ruta.IdRepartidor))
                ModelState.AddModelError("IdRepartidor", "Debes seleccionar un repartidor activo.");

            if (ModelState.IsValid)
            {
                _rutaService.Create(ruta);
                return RedirectToAction("Index");
            }

            CargarCombos(ruta);
            return View(ruta);
        }

        [RoleAuthorize(RoleNames.Admin)]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ruta = _rutaService.GetById(id.Value);
            if (ruta == null) return HttpNotFound();

            CargarCombos(ruta);
            return View(ruta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(RoleNames.Admin)]
        public ActionResult Edit([Bind(Include = "IdRuta,FechaRuta,IdRepartidor,IdEstadoRuta,Observacion,FechaCreacion")] Ruta ruta)
        {
            if (!_estadoService.IsValidEstadoRuta(ruta.IdEstadoRuta))
                ModelState.AddModelError("IdEstadoRuta", "Debes seleccionar un estado valido para Ruta.");

            if (!_rutaService.IsRepartidorActivo(ruta.IdRepartidor))
                ModelState.AddModelError("IdRepartidor", "Debes seleccionar un repartidor activo.");

            if (ModelState.IsValid)
            {
                _rutaService.Update(ruta);
                return RedirectToAction("Index");
            }

            CargarCombos(ruta);
            return View(ruta);
        }

        [RoleAuthorize(RoleNames.Admin)]
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ruta = _rutaService.GetByIdWithEstadoAndRepartidor(id.Value);
            if (ruta == null) return HttpNotFound();

            return View(ruta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(RoleNames.Admin)]
        public ActionResult DeleteConfirmed(int id)
        {
            _rutaService.Delete(id);
            return RedirectToAction("Index");
        }

        [RoleAuthorize(RoleNames.Repartidor)]
        public ActionResult Inicio()
        {
            int idRepartidor = CurrentUser.IdRepartidor ?? 0;
            if (idRepartidor <= 0) return RedirectToAction("Login", "Account");

            var vm = _rutaService.GetInicioData(idRepartidor);
            return View(vm);
        }

        [RoleAuthorize(RoleNames.Admin)]
        public ActionResult Asignar(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var vm = _rutaService.GetAsignarData(id.Value);
            if (vm == null) return HttpNotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorize(RoleNames.Admin)]
        public ActionResult Asignar(int IdRuta, List<int> PaquetesSeleccionados)
        {
            _rutaService.SaveAsignacion(IdRuta, PaquetesSeleccionados);
            return RedirectToAction("Details", new { id = IdRuta });
        }

        private void CargarCombos(Ruta ruta = null)
        {
            ViewBag.IdEstadoRuta = _dropdownService.EstadosRuta(ruta?.IdEstadoRuta);
            ViewBag.IdRepartidor = _dropdownService.RepartidoresActivos(ruta?.IdRepartidor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
