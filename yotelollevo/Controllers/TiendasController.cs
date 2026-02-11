using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using yotelollevo.Constants;
using yotelollevo.Filter;
using yotelollevo.Services;

namespace yotelollevo.Controllers
{
    [RoleAuthorize(RoleNames.Admin)]
    public class TiendasController : BaseController
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();
        private readonly IDropdownService _dropdownService;

        public TiendasController()
        {
            _dropdownService = new DropdownService(db);
        }

        public ActionResult Index()
        {
            var tiendas = db.Tienda
                .Include(t => t.Comuna)
                .OrderBy(t => t.Nombre)
                .ToList();

            return View(tiendas);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tienda = db.Tienda
                .Include(t => t.Comuna)
                .FirstOrDefault(t => t.IdTienda == id);

            if (tienda == null) return HttpNotFound();

            return View(tienda);
        }

        public ActionResult Create()
        {
            ViewBag.IdComuna = _dropdownService.Comunas();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTienda,Nombre,Correo,Direccion,Telefono,IdComuna,Activa")] Tienda tienda)
        {
            if (ModelState.IsValid)
            {
                tienda.FechaCreacion = DateTime.Now;
                db.Tienda.Add(tienda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdComuna = _dropdownService.Comunas(tienda.IdComuna);
            return View(tienda);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tienda = db.Tienda.Find(id);
            if (tienda == null) return HttpNotFound();

            ViewBag.IdComuna = _dropdownService.Comunas(tienda.IdComuna);
            return View(tienda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTienda,Nombre,Correo,Direccion,Telefono,IdComuna,Activa,FechaCreacion")] Tienda tienda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tienda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdComuna = _dropdownService.Comunas(tienda.IdComuna);
            return View(tienda);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var tienda = db.Tienda
                .Include(t => t.Comuna)
                .FirstOrDefault(t => t.IdTienda == id);

            if (tienda == null) return HttpNotFound();

            return View(tienda);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var tienda = db.Tienda.Find(id);
            if (tienda != null)
            {
                db.Tienda.Remove(tienda);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
