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
    [RoleAuthorize("ADMIN", "TIENDA")]
    public class PaquetesController : Controller
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();

        // GET: Paquetes
        public ActionResult Index()
        {
            // Traer paquetes con relaciones
            var paquetes = db.Paquete
                .Include(p => p.Tienda)
                .Include(p => p.Comuna)
                .Include(p => p.Estado)
                .ToList();

            // Si tu tabla Estado no tiene Tipo, borra el Where
            var tiendas = db.Tienda
                .OrderBy(t => t.Nombre)
                .Select(t => t.Nombre)
                .Distinct()
                .ToList();

            var estados = db.Estado
                //.Where(e => e.Tipo == "Paquete")  // comenta si no existe Tipo
                .OrderBy(e => e.Nombre)
                .Select(e => e.Nombre)
                .Distinct()
                .ToList();

            ViewBag.Tiendas = tiendas;
            ViewBag.Estados = estados;

            return View(paquetes);
        }



        // GET: Paquetes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paquete paquete = db.Paquete.Find(id);
            if (paquete == null)
            {
                return HttpNotFound();
            }
            return View(paquete);
        }

        // GET: Paquetes/Create
        public ActionResult Create()
        {
            ViewBag.IdTienda = new SelectList(db.Tienda, "IdTienda", "Nombre");
            ViewBag.IdComunaEntrega = new SelectList(db.Comuna, "IdComuna", "Nombre");

            // Estados tipo Pedido
            ViewBag.IdEstadoPedido = new SelectList(
                db.Estado.Where(e => e.Tipo == "Pedido"),
                "IdEstado",
                "Nombre"
            );

            return View();
        }


        // POST: Paquetes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPaquete,IdTienda,NombreDestinatario,DireccionEntrega,IdComunaEntrega,CelularDestinatario,Comentario,IdEstadoPedido,FechaCreacion")] Paquete paquete)
        {
            if (ModelState.IsValid)
            {
                db.Paquete.Add(paquete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdTienda = new SelectList(db.Tienda, "IdTienda", "Nombre", paquete.IdTienda);
            ViewBag.IdComunaEntrega = new SelectList(db.Comuna, "IdComuna", "Nombre", paquete.IdComunaEntrega);
            ViewBag.IdEstadoPedido = new SelectList(db.Estado.Where(e => e.Tipo == "Pedido"), "IdEstado", "Nombre", paquete.IdEstadoPedido);
            return View(paquete);
        }

        // GET: Paquetes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paquete paquete = db.Paquete.Find(id);
            if (paquete == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdComunaEntrega = new SelectList(db.Comuna, "IdComuna", "Nombre", paquete.IdComunaEntrega);
            ViewBag.IdEstadoPedido = new SelectList(db.Estado, "IdEstado", "Tipo", paquete.IdEstadoPedido);
            ViewBag.IdTienda = new SelectList(db.Tienda, "IdTienda", "Nombre", paquete.IdTienda);
            return View(paquete);
        }

        // POST: Paquetes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPaquete,IdTienda,NombreDestinatario,DireccionEntrega,IdComunaEntrega,CelularDestinatario,Comentario,IdEstadoPedido,FechaCreacion")] Paquete paquete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paquete).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdComunaEntrega = new SelectList(db.Comuna, "IdComuna", "Nombre", paquete.IdComunaEntrega);
            ViewBag.IdEstadoPedido = new SelectList(db.Estado, "IdEstado", "Tipo", paquete.IdEstadoPedido);
            ViewBag.IdTienda = new SelectList(db.Tienda, "IdTienda", "Nombre", paquete.IdTienda);
            return View(paquete);
        }

        // GET: Paquetes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paquete paquete = db.Paquete.Find(id);
            if (paquete == null)
            {
                return HttpNotFound();
            }
            return View(paquete);
        }

        // POST: Paquetes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paquete paquete = db.Paquete.Find(id);
            db.Paquete.Remove(paquete);
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
