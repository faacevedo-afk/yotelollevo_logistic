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
    public class TiendasController : Controller
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();

        // GET: Tiendas
        public ActionResult Index()
        {
            var tienda = db.Tienda.Include(t => t.Comuna);
            return View(tienda.ToList());
        }

        // GET: Tiendas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tienda tienda = db.Tienda.Find(id);
            if (tienda == null)
            {
                return HttpNotFound();
            }
            return View(tienda);
        }

        // GET: Tiendas/Create
        public ActionResult Create()
        {
            ViewBag.IdComuna = new SelectList(db.Comuna, "IdComuna", "Nombre");
            return View();
        }

        // POST: Tiendas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
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

            ViewBag.IdComuna = new SelectList(db.Comuna, "IdComuna", "Nombre", tienda.IdComuna);
            return View(tienda);
        }


        // GET: Tiendas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tienda tienda = db.Tienda.Find(id);
            if (tienda == null)
            {
                return HttpNotFound();
            }

            // Cargar comunas y dejar seleccionada la actual
            ViewBag.IdComuna = new SelectList(db.Comuna, "IdComuna", "Nombre", tienda.IdComuna);

            return View(tienda);
        }


        // POST: Tiendas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTienda,Nombre,Correo,Direccion,Telefono,IdComuna,Activa")] Tienda tienda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tienda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Volver a cargar comunas si hay error
            ViewBag.IdComuna = new SelectList(db.Comuna, "IdComuna", "Nombre", tienda.IdComuna);
            return View(tienda);
        }


        // GET: Tiendas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tienda tienda = db.Tienda.Find(id);
            if (tienda == null)
            {
                return HttpNotFound();
            }
            return View(tienda);
        }

        // POST: Tiendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tienda tienda = db.Tienda.Find(id);
            db.Tienda.Remove(tienda);
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
