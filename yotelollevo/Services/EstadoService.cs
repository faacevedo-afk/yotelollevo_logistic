using System.Collections.Generic;
using System.Linq;
using yotelollevo.Constants;

namespace yotelollevo.Services
{
    public class EstadoService : IEstadoService
    {
        private readonly LogisticaDBEntities _db;

        public EstadoService(LogisticaDBEntities db)
        {
            _db = db;
        }

        public int GetEstadoId(string nombre)
        {
            return _db.Estado
                .Where(e => e.Nombre == nombre)
                .Select(e => e.IdEstado)
                .FirstOrDefault();
        }

        public List<int> GetEstadoIds(params string[] nombres)
        {
            return _db.Estado
                .Where(e => nombres.Contains(e.Nombre))
                .Select(e => e.IdEstado)
                .ToList();
        }

        public bool IsValidEstadoPedido(int idEstado)
        {
            return _db.Estado.Any(e => e.IdEstado == idEstado && EstadoNames.EstadosPedido.Contains(e.Nombre));
        }

        public bool IsValidEstadoRuta(int idEstado)
        {
            return _db.Estado.Any(e => e.IdEstado == idEstado && EstadoNames.EstadosRuta.Contains(e.Nombre));
        }
    }
}
