using System;
using System.Net;
using System.Web.Mvc;
using yotelollevo.Constants;
using yotelollevo.Filter;
using yotelollevo.Services;

namespace yotelollevo.Controllers
{
    [RoleAuthorize(RoleNames.Admin, RoleNames.Tienda)]
    public class PaquetesController : BaseController
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();
        private readonly IPaqueteService _paqueteService;
        private readonly IEstadoService _estadoService;
        private readonly IDropdownService _dropdownService;

        public PaquetesController()
        {
            _paqueteService = new PaqueteService(db);
            _estadoService = new EstadoService(db);
            _dropdownService = new DropdownService(db);
        }

        public ActionResult Index()
        {
            int? tiendaFilter = CurrentUser.IsTienda ? CurrentUser.IdTienda : (int?)null;
            var paquetes = _paqueteService.GetAll(tiendaFilter);

            ViewBag.Tiendas = _paqueteService.GetTiendaNames();
            ViewBag.Estados = _paqueteService.GetEstadoPedidoNames();

            return View(paquetes);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var paquete = _paqueteService.GetByIdWithIncludes(id.Value);
            if (paquete == null) return HttpNotFound();

            if (CurrentUser.IsTienda && paquete.IdTienda != CurrentUser.IdTienda)
                return new HttpStatusCodeResult(403);

            return View(paquete);
        }

        public ActionResult Create()
        {
            CargarCombos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPaquete,IdTienda,NombreDestinatario,DireccionEntrega,IdComunaEntrega,CelularDestinatario,Comentario,IdEstadoPedido,FechaCreacion")] Paquete paquete)
        {
            if (CurrentUser.IsTienda && CurrentUser.IdTienda.HasValue)
                paquete.IdTienda = CurrentUser.IdTienda.Value;

            if (paquete.FechaCreacion == default(DateTime))
                paquete.FechaCreacion = DateTime.Now;

            if (paquete.IdEstadoPedido <= 0 || !_estadoService.IsValidEstadoPedido(paquete.IdEstadoPedido))
                ModelState.AddModelError("IdEstadoPedido", "Debes seleccionar un estado de pedido valido.");

            if (ModelState.IsValid)
            {
                _paqueteService.Create(paquete);
                return RedirectToAction("Index");
            }

            CargarCombos(paquete);
            return View(paquete);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var paquete = _paqueteService.GetById(id.Value);
            if (paquete == null) return HttpNotFound();

            if (CurrentUser.IsTienda && paquete.IdTienda != CurrentUser.IdTienda)
                return new HttpStatusCodeResult(403);

            CargarCombos(paquete);
            return View(paquete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPaquete,IdTienda,NombreDestinatario,DireccionEntrega,IdComunaEntrega,CelularDestinatario,Comentario,IdEstadoPedido,FechaCreacion")] Paquete paquete)
        {
            if (CurrentUser.IsTienda && paquete.IdTienda != CurrentUser.IdTienda)
                return new HttpStatusCodeResult(403);

            if (paquete.IdEstadoPedido <= 0 || !_estadoService.IsValidEstadoPedido(paquete.IdEstadoPedido))
                ModelState.AddModelError("IdEstadoPedido", "Debes seleccionar un estado de pedido valido.");

            if (ModelState.IsValid)
            {
                _paqueteService.Update(paquete);
                return RedirectToAction("Index");
            }

            CargarCombos(paquete);
            return View(paquete);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var paquete = _paqueteService.GetByIdWithIncludes(id.Value);
            if (paquete == null) return HttpNotFound();

            if (CurrentUser.IsTienda && paquete.IdTienda != CurrentUser.IdTienda)
                return new HttpStatusCodeResult(403);

            return View(paquete);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (CurrentUser.IsTienda)
            {
                var paquete = _paqueteService.GetById(id);
                if (paquete != null && paquete.IdTienda != CurrentUser.IdTienda)
                    return new HttpStatusCodeResult(403);
            }

            _paqueteService.Delete(id);
            return RedirectToAction("Index");
        }

        private void CargarCombos(Paquete paquete = null)
        {
            ViewBag.IdTienda = _dropdownService.Tiendas(paquete?.IdTienda);
            ViewBag.IdComunaEntrega = _dropdownService.Comunas(paquete?.IdComunaEntrega);
            ViewBag.IdEstadoPedido = _dropdownService.EstadosPedido(paquete?.IdEstadoPedido);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
