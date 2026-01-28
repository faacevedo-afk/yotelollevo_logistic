using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using yotelollevo;

namespace yotelollevo.Controllers
{
    public class RepartidorsController : Controller
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();

        // GET: Repartidors
        public ActionResult Index()
        {
            return View(db.Repartidor.ToList());
        }

        // GET: Repartidors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repartidor repartidor = db.Repartidor.Find(id);
            if (repartidor == null)
            {
                return HttpNotFound();
            }
            return View(repartidor);
        }

        // GET: Repartidors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Repartidors/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Repartidors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repartidor repartidor = db.Repartidor.Find(id);
            if (repartidor == null)
            {
                return HttpNotFound();
            }
            return View(repartidor);
        }

        // POST: Repartidors/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Repartidors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Repartidor repartidor = db.Repartidor.Find(id);
            if (repartidor == null)
            {
                return HttpNotFound();
            }
            return View(repartidor);
        }

        // POST: Repartidors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Repartidor repartidor = db.Repartidor.Find(id);
            db.Repartidor.Remove(repartidor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
