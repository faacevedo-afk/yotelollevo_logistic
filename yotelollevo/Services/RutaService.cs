using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using yotelollevo.Constants;
using yotelollevo.ViewModels;

namespace yotelollevo.Services
{
    public class RutaService : IRutaService
    {
        private readonly LogisticaDBEntities _db;
        private readonly IEstadoService _estadoService;

        public RutaService(LogisticaDBEntities db, IEstadoService estadoService)
        {
            _db = db;
            _estadoService = estadoService;
        }

        public List<Ruta> GetAll()
        {
            return _db.Ruta
                .Include(r => r.Estado)
                .Include(r => r.Repartidor)
                .Include(r => r.RutaPaquete)
                .OrderByDescending(r => r.FechaCreacion)
                .ToList();
        }

        public Ruta GetByIdWithDetails(int id)
        {
            return _db.Ruta
                .Include(r => r.Estado)
                .Include(r => r.Repartidor)
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete))
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete.Tienda))
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete.Comuna))
                .FirstOrDefault(r => r.IdRuta == id);
        }

        public Ruta GetById(int id)
        {
            return _db.Ruta.Find(id);
        }

        public Ruta GetByIdWithEstadoAndRepartidor(int id)
        {
            return _db.Ruta
                .Include(r => r.Estado)
                .Include(r => r.Repartidor)
                .Include(r => r.RutaPaquete)
                .FirstOrDefault(r => r.IdRuta == id);
        }

        public void Create(Ruta ruta)
        {
            ruta.FechaCreacion = DateTime.Now;
            ruta.IdEstadoRuta = _estadoService.GetEstadoId(EstadoNames.Planificada);
            _db.Ruta.Add(ruta);
            _db.SaveChanges();
        }

        public void Detach(Ruta ruta)
        {
            _db.Entry(ruta).State = EntityState.Detached;
        }

        public void Update(Ruta ruta)
        {
            _db.Entry(ruta).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var ruta = _db.Ruta
                .Include(r => r.RutaPaquete)
                .FirstOrDefault(r => r.IdRuta == id);
            if (ruta != null)
            {
                int idCreado = _estadoService.GetEstadoId(EstadoNames.Creado);
                foreach (var rp in ruta.RutaPaquete.ToList())
                {
                    var paquete = _db.Paquete.Find(rp.IdPaquete);
                    if (paquete != null)
                        paquete.IdEstadoPedido = idCreado;

                    _db.RutaPaquete.Remove(rp);
                }

                _db.Ruta.Remove(ruta);
                _db.SaveChanges();
            }
        }

        public bool IsRepartidorActivo(int idRepartidor)
        {
            return _db.Repartidor.Any(r => r.IdRepartidor == idRepartidor && r.Activo == true);
        }

        public MiRutaInicioVM GetInicioData(int idRepartidor)
        {
            var idsActivos = _estadoService.GetEstadoIds(EstadoNames.Planificada, EstadoNames.EnCurso);
            int estadoCerradaId = _estadoService.GetEstadoId(EstadoNames.Cerrada);

            var rutas = _db.Ruta
                .Include(r => r.Estado)
                .Where(r => r.IdRepartidor == idRepartidor)
                .ToList();

            return new MiRutaInicioVM
            {
                RutaActiva = rutas
                    .Where(r => idsActivos.Contains(r.IdEstadoRuta))
                    .OrderByDescending(r => r.FechaRuta)
                    .FirstOrDefault(),
                Historial = rutas
                    .Where(r => r.IdEstadoRuta == estadoCerradaId)
                    .OrderByDescending(r => r.FechaRuta)
                    .ToList()
            };
        }

        public AsignarPaquetesVM GetAsignarData(int idRuta)
        {
            var ruta = _db.Ruta
                .Include(r => r.Repartidor)
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete))
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete.Tienda))
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete.Comuna))
                .FirstOrDefault(r => r.IdRuta == idRuta);

            if (ruta == null) return null;

            int idCreado = _estadoService.GetEstadoId(EstadoNames.Creado);

            var pendientes = _db.Paquete
                .Include(p => p.Tienda)
                .Include(p => p.Comuna)
                .Where(p => p.IdEstadoPedido == idCreado)
                .ToList()
                .Select(p => new ItemPaquete
                {
                    IdPaquete = p.IdPaquete,
                    Tienda = p.Tienda != null ? p.Tienda.Nombre : "",
                    Destinatario = p.NombreDestinatario,
                    Direccion = p.DireccionEntrega,
                    Comuna = p.Comuna != null ? p.Comuna.Nombre : ""
                })
                .ToList();

            var yaEnRuta = ruta.RutaPaquete
                .Select(rp => new ItemPaquete
                {
                    IdPaquete = rp.Paquete.IdPaquete,
                    Tienda = rp.Paquete.Tienda != null ? rp.Paquete.Tienda.Nombre : "",
                    Destinatario = rp.Paquete.NombreDestinatario,
                    Direccion = rp.Paquete.DireccionEntrega,
                    Comuna = rp.Paquete.Comuna != null ? rp.Paquete.Comuna.Nombre : ""
                })
                .ToList();

            return new AsignarPaquetesVM
            {
                IdRuta = ruta.IdRuta,
                RutaInfo = string.Format("Ruta #{0} - {1} ({2})",
                    ruta.IdRuta,
                    ruta.Repartidor != null ? ruta.Repartidor.Nombre : "Sin repartidor",
                    ruta.FechaRuta.ToString("dd-MM-yyyy")),
                Pendientes = pendientes,
                YaEnRuta = yaEnRuta
            };
        }

        public void SaveAsignacion(int idRuta, List<int> paquetesSeleccionados)
        {
            if (paquetesSeleccionados == null || !paquetesSeleccionados.Any()) return;

            int idAsignado = _estadoService.GetEstadoId(EstadoNames.Asignado);
            int maxOrden = _db.RutaPaquete
                .Where(rp => rp.IdRuta == idRuta)
                .Select(rp => (int?)rp.OrdenEntrega)
                .Max() ?? 0;

            foreach (var idPaquete in paquetesSeleccionados)
            {
                bool yaExiste = _db.RutaPaquete.Any(rp => rp.IdRuta == idRuta && rp.IdPaquete == idPaquete);
                if (!yaExiste)
                {
                    maxOrden++;
                    _db.RutaPaquete.Add(new RutaPaquete
                    {
                        IdRuta = idRuta,
                        IdPaquete = idPaquete,
                        OrdenEntrega = maxOrden,
                        IdEstadoEntrega = idAsignado
                    });

                    var paquete = _db.Paquete.Find(idPaquete);
                    if (paquete != null && idAsignado > 0)
                        paquete.IdEstadoPedido = idAsignado;
                }
            }

            _db.SaveChanges();
        }

        public TrabajarRutaVM GetTrabajarData(int idRuta)
        {
            var ruta = _db.Ruta
                .Include(r => r.Estado)
                .Include(r => r.RutaPaquete.Select(rp => rp.Estado))
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete))
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete.Tienda))
                .Include(r => r.RutaPaquete.Select(rp => rp.Paquete.Comuna))
                .FirstOrDefault(r => r.IdRuta == idRuta);

            if (ruta == null) return null;

            string estadoNombre = ruta.Estado != null ? ruta.Estado.Nombre : "";
            int idEntregado = _estadoService.GetEstadoId(EstadoNames.Entregado);
            int idFallido = _estadoService.GetEstadoId(EstadoNames.Fallido);

            return new TrabajarRutaVM
            {
                IdRuta = ruta.IdRuta,
                FechaRuta = ruta.FechaRuta,
                EstadoRuta = estadoNombre,
                PuedeIniciar = estadoNombre == EstadoNames.Planificada,
                PuedeCerrar = estadoNombre == EstadoNames.EnCurso,
                Paquetes = ruta.RutaPaquete
                    .OrderBy(rp => rp.OrdenEntrega)
                    .Select(rp => new TrabajoPaqueteItem
                    {
                        IdRutaPaquete = rp.IdRutaPaquete,
                        IdPaquete = rp.IdPaquete,
                        OrdenEntrega = rp.OrdenEntrega,
                        Destinatario = rp.Paquete.NombreDestinatario,
                        Direccion = rp.Paquete.DireccionEntrega,
                        Comuna = rp.Paquete.Comuna != null ? rp.Paquete.Comuna.Nombre : "",
                        Tienda = rp.Paquete.Tienda != null ? rp.Paquete.Tienda.Nombre : "",
                        EstadoEntrega = rp.Estado != null ? rp.Estado.Nombre : "",
                        Observacion = rp.Observacion,
                        HoraEvento = rp.HoraEvento,
                        YaFinalizado = rp.IdEstadoEntrega == idEntregado || rp.IdEstadoEntrega == idFallido
                    })
                    .ToList()
            };
        }

        public void CambiarEstadoRuta(int idRuta, string nuevoEstado)
        {
            var ruta = _db.Ruta
                .Include(r => r.Estado)
                .Include(r => r.RutaPaquete)
                .FirstOrDefault(r => r.IdRuta == idRuta);
            if (ruta == null) return;

            string estadoActual = ruta.Estado != null ? ruta.Estado.Nombre : "";

            bool transicionValida =
                (estadoActual == EstadoNames.Planificada && nuevoEstado == EstadoNames.EnCurso) ||
                (estadoActual == EstadoNames.EnCurso && nuevoEstado == EstadoNames.Cerrada);

            if (!transicionValida) return;

            int idNuevoEstado = _estadoService.GetEstadoId(nuevoEstado);
            ruta.IdEstadoRuta = idNuevoEstado;

            if (nuevoEstado == EstadoNames.EnCurso)
            {
                int idEnRuta = _estadoService.GetEstadoId(EstadoNames.EnRuta);
                foreach (var rp in ruta.RutaPaquete)
                {
                    var paquete = _db.Paquete.Find(rp.IdPaquete);
                    if (paquete != null)
                        paquete.IdEstadoPedido = idEnRuta;
                }
            }

            _db.SaveChanges();
        }

        public void MarcarEntrega(int idRutaPaquete, string nuevoEstado, string observacion)
        {
            var rp = _db.RutaPaquete
                .Include(x => x.Estado)
                .FirstOrDefault(x => x.IdRutaPaquete == idRutaPaquete);
            if (rp == null) return;

            int idEntregado = _estadoService.GetEstadoId(EstadoNames.Entregado);
            int idFallido = _estadoService.GetEstadoId(EstadoNames.Fallido);

            bool yaFinalizado = rp.IdEstadoEntrega == idEntregado || rp.IdEstadoEntrega == idFallido;
            if (yaFinalizado) return;

            if (nuevoEstado != EstadoNames.Entregado && nuevoEstado != EstadoNames.Fallido) return;

            int idNuevoEstado = _estadoService.GetEstadoId(nuevoEstado);
            rp.IdEstadoEntrega = idNuevoEstado;
            rp.HoraEvento = DateTime.Now;
            rp.Observacion = observacion;

            var paquete = _db.Paquete.Find(rp.IdPaquete);
            if (paquete != null)
                paquete.IdEstadoPedido = idNuevoEstado;

            _db.SaveChanges();
        }
    }
}
