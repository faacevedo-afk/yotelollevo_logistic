using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using System.Net;
using System.Web;
using System.Web.Mvc;
using yotelollevo.ViewsModel;
using yotelollevo;



namespace yotelollevo.Controllers
{
    [RoleAuthorize("ADMIN", "REPARTIDOR")]
    public class RutasController : Controller
    {
        private LogisticaDBEntities db = new LogisticaDBEntities();

        // GET: Rutas
        public ActionResult Index()
        {
            var ruta = db.Ruta.Include(r => r.Estado).Include(r => r.Repartidor);
            return View(ruta.ToList());
        }

        // GET: Rutas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var ruta = db.Ruta
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete))
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete.Tienda))
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete.Comuna))
                .FirstOrDefault(r => r.IdRuta == id);

            if (ruta == null) return HttpNotFound();

            return View(ruta);
        }

        // GET: Rutas/Create
        public ActionResult Create()
        {
            ViewBag.IdEstadoRuta = new SelectList(db.Estado, "IdEstado", "Tipo");
            ViewBag.IdRepartidor = new SelectList(db.Repartidor, "IdRepartidor", "Nombre");
            return View();
        }

        // POST: Rutas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdRuta,FechaRuta,IdRepartidor")] Ruta ruta)
        {
            if (ModelState.IsValid)
            {
                // 1) Fecha creación automática
                ruta.FechaCreacion = DateTime.Now;

                // 2) Estado por defecto: Planificada
                ruta.IdEstadoRuta = db.Estado
                    .Where(e => e.Tipo == "Ruta" && e.Nombre == "Planificada")
                    .Select(e => e.IdEstado)
                    .FirstOrDefault();

                db.Ruta.Add(ruta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Si falla, hay que recargar el dropdown
            ViewBag.IdRepartidor = new SelectList(
                db.Repartidor.Where(r => r.Activo == true),
                "IdRepartidor",
                "Nombre",
                ruta.IdRepartidor
            );

            return View(ruta);
        }



        // GET: Rutas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ruta ruta = db.Ruta.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEstadoRuta = new SelectList(db.Estado, "IdEstado", "Tipo", ruta.IdEstadoRuta);
            ViewBag.IdRepartidor = new SelectList(db.Repartidor, "IdRepartidor", "Nombre", ruta.IdRepartidor);
            return View(ruta);
        }

        // POST: Rutas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdRuta,FechaRuta,IdRepartidor,IdEstadoRuta,Observacion,FechaCreacion")] Ruta ruta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ruta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdEstadoRuta = new SelectList(db.Estado, "IdEstado", "Tipo", ruta.IdEstadoRuta);
            ViewBag.IdRepartidor = new SelectList(db.Repartidor, "IdRepartidor", "Nombre", ruta.IdRepartidor);
            return View(ruta);
        }

        // GET: Rutas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ruta ruta = db.Ruta.Find(id);
            if (ruta == null)
            {
                return HttpNotFound();
            }
            return View(ruta);
        }

        // POST: Rutas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ruta ruta = db.Ruta.Find(id);
            db.Ruta.Remove(ruta);
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
 
    

        //parte asignacion
        public ActionResult Asignar(int id)
        {
            var ruta = db.Ruta.Find(id);
            if (ruta == null) return HttpNotFound();

            // Estado Pedido = Creado
            var estadoCreadoId = db.Estado
                .Where(e => e.Tipo == "Pedido" && e.Nombre == "Creado")
                .Select(e => e.IdEstado)
                .FirstOrDefault();

            var vm = new AsignarPaquetesVM
            {
                IdRuta = id,
                RutaInfo = $"Ruta #{ruta.IdRuta} - {ruta.FechaRuta:dd-MM-yyyy}"
            };

            // Paquetes pendientes
            vm.Pendientes = db.Paquete
                .Where(p => p.IdEstadoPedido == estadoCreadoId)
                .Select(p => new ItemPaquete
                {
                    IdPaquete = p.IdPaquete,
                    Tienda = p.Tienda.Nombre,
                    Destinatario = p.NombreDestinatario,
                    Direccion = p.DireccionEntrega,
                    Comuna = p.Comuna.Nombre
                })
                .ToList();

            // Ya asignados a la ruta
            vm.YaEnRuta = db.RutaPaquete
                .Where(rp => rp.IdRuta == id)
                .OrderBy(rp => rp.OrdenEntrega)
                .Select(rp => new ItemPaquete
                {
                    IdPaquete = rp.IdPaquete,
                    Tienda = rp.Paquete.Tienda.Nombre,
                    Destinatario = rp.Paquete.NombreDestinatario,
                    Direccion = rp.Paquete.DireccionEntrega,
                    Comuna = rp.Paquete.Comuna.Nombre
                })
                .ToList();

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Asignar(AsignarPaquetesVM vm)
        {
            if (vm == null) return RedirectToAction("Index");

            // Estado Entrega = Pendiente
            var estadoPendienteEntregaId = db.Estado
                .Where(e => e.Tipo == "Entrega" && e.Nombre == "Pendiente")
                .Select(e => e.IdEstado)
                .FirstOrDefault();

            // Estado Pedido = Asignado
            var estadoAsignadoPedidoId = db.Estado
                .Where(e => e.Tipo == "Pedido" && e.Nombre == "Asignado")
                .Select(e => e.IdEstado)
                .FirstOrDefault();

            int ultimoOrden = db.RutaPaquete
                .Where(x => x.IdRuta == vm.IdRuta)
                .Select(x => (int?)x.OrdenEntrega)
                .Max() ?? 0;

            foreach (var idPaquete in (vm.PaquetesSeleccionados ?? new System.Collections.Generic.List<int>()).Distinct())
            {
                bool yaExiste = db.RutaPaquete.Any(x => x.IdRuta == vm.IdRuta && x.IdPaquete == idPaquete);
                if (yaExiste) continue;

                ultimoOrden++;

                db.RutaPaquete.Add(new RutaPaquete
                {
                    IdRuta = vm.IdRuta,
                    IdPaquete = idPaquete,
                    OrdenEntrega = ultimoOrden,
                    IdEstadoEntrega = estadoPendienteEntregaId
                });

                var paquete = db.Paquete.Find(idPaquete);
                if (paquete != null)
                    paquete.IdEstadoPedido = estadoAsignadoPedidoId;
            }

            db.SaveChanges();
            return RedirectToAction("Asignar", new { id = vm.IdRuta });
        }

    }
}

