using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using yotelollevo.Constants;

namespace yotelollevo.Services
{
    public class PaqueteService : IPaqueteService
    {
        private readonly LogisticaDBEntities _db;

        public PaqueteService(LogisticaDBEntities db)
        {
            _db = db;
        }

        public List<Paquete> GetAll(int? idTiendaFilter = null)
        {
            var query = _db.Paquete
                .Include(p => p.Tienda)
                .Include(p => p.Comuna)
                .Include(p => p.Estado)
                .AsQueryable();

            if (idTiendaFilter.HasValue)
                query = query.Where(p => p.IdTienda == idTiendaFilter.Value);

            return query.ToList();
        }

        public Paquete GetById(int id)
        {
            return _db.Paquete.Find(id);
        }

        public Paquete GetByIdWithIncludes(int id)
        {
            return _db.Paquete
                .Include(p => p.Tienda)
                .Include(p => p.Comuna)
                .Include(p => p.Estado)
                .FirstOrDefault(p => p.IdPaquete == id);
        }

        public void Create(Paquete paquete)
        {
            if (paquete.FechaCreacion == default(DateTime))
                paquete.FechaCreacion = DateTime.Now;

            _db.Paquete.Add(paquete);
            _db.SaveChanges();
        }

        public void Update(Paquete paquete)
        {
            _db.Entry(paquete).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var paquete = _db.Paquete.Find(id);
            if (paquete != null)
            {
                _db.Paquete.Remove(paquete);
                _db.SaveChanges();
            }
        }

        public List<string> GetTiendaNames()
        {
            return _db.Tienda
                .OrderBy(t => t.Nombre)
                .Select(t => t.Nombre)
                .Distinct()
                .ToList();
        }

        public List<string> GetEstadoPedidoNames()
        {
            return _db.Estado
                .Where(e => EstadoNames.EstadosPedido.Contains(e.Nombre))
                .OrderBy(e => e.Nombre)
                .Select(e => e.Nombre)
                .Distinct()
                .ToList();
        }

        public List<yotelollevo.ViewModels.PaqueteViewModel> GetRecentForDashboard(int count = 10)
        {
            return _db.Paquete
                .Include(p => p.Tienda)
                .Include(p => p.Estado)
                .Include(p => p.Comuna)
                .OrderByDescending(p => p.IdPaquete)
                .Take(count)
                .Select(p => new yotelollevo.ViewModels.PaqueteViewModel
                {
                    IdPaquete = p.IdPaquete,
                    Destinatario = p.NombreDestinatario,
                    TiendaNombre = p.Tienda != null ? p.Tienda.Nombre : "",
                    DireccionEntrega = p.DireccionEntrega,
                    EstadoNombre = p.Estado != null ? p.Estado.Nombre : "",
                    CodigoProducto = p.IdPaquete.ToString(),
                })
                .ToList();
        }
    }
}
