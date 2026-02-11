using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using yotelollevo.Constants;
using yotelollevo.Filter;

namespace yotelollevo.Controllers
{
    [RoleAuthorize(RoleNames.Admin)]
    public class RepartidorsController : BaseController
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();

        public ActionResult Index()
        {
            return View(db.Repartidor.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Repartidor repartidor = db.Repartidor.Find(id);
            if (repartidor == null) return HttpNotFound();

            return View(repartidor);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRepartidor,Nombre,ApellidoPaterno,ApellidoMaterno,Rut,Correo,Celular,Activo,FechaCreacion")] Repartidor repartidor)
        {
            if (ModelState.IsValid)
            {
                repartidor.FechaCreacion = DateTime.Now;
                db.Repartidor.Add(repartidor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(repartidor);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Repartidor repartidor = db.Repartidor.Find(id);
            if (repartidor == null) return HttpNotFound();

            return View(repartidor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRepartidor,Nombre,ApellidoPaterno,ApellidoMaterno,Rut,Correo,Celular,Activo,FechaCreacion")] Repartidor repartidor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(repartidor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(repartidor);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Repartidor repartidor = db.Repartidor.Find(id);
            if (repartidor == null) return HttpNotFound();

            return View(repartidor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Repartidor repartidor = db.Repartidor.Find(id);
            if (repartidor != null)
            {
                db.Repartidor.Remove(repartidor);
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
