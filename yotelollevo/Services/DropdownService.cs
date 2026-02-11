using System.Linq;
using System.Web.Mvc;
using yotelollevo.Constants;

namespace yotelollevo.Services
{
    public class DropdownService : IDropdownService
    {
        private readonly LogisticaDBEntities _db;

        public DropdownService(LogisticaDBEntities db)
        {
            _db = db;
        }

        public SelectList Tiendas(int? selected = null)
        {
            return new SelectList(
                _db.Tienda.OrderBy(t => t.Nombre),
                "IdTienda",
                "Nombre",
                selected);
        }

        public SelectList Comunas(int? selected = null)
        {
            return new SelectList(
                _db.Comuna.OrderBy(c => c.Nombre),
                "IdComuna",
                "Nombre",
                selected);
        }

        public SelectList EstadosPedido(int? selected = null)
        {
            return new SelectList(
                _db.Estado.Where(e => EstadoNames.EstadosPedido.Contains(e.Nombre)).OrderBy(e => e.Nombre),
                "IdEstado",
                "Nombre",
                selected);
        }

        public SelectList EstadosRuta(int? selected = null)
        {
            return new SelectList(
                _db.Estado.Where(e => EstadoNames.EstadosRuta.Contains(e.Nombre)).OrderBy(e => e.Nombre),
                "IdEstado",
                "Nombre",
                selected);
        }

        public SelectList RepartidoresActivos(int? selected = null)
        {
            return new SelectList(
                _db.Repartidor.Where(r => r.Activo == true).OrderBy(r => r.Nombre),
                "IdRepartidor",
                "Nombre",
                selected);
        }
    }
}
