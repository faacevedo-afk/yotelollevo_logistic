using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using yotelollevo.ViewModels;

namespace yotelollevo.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly LogisticaDBEntities _db;
        private readonly IPaqueteService _paqueteService;

        public DashboardService(LogisticaDBEntities db, IPaqueteService paqueteService)
        {
            _db = db;
            _paqueteService = paqueteService;
        }

        public HomeIndexViewModel GetDashboardData()
        {
            return new HomeIndexViewModel
            {
                TotalPaquetes = _db.Paquete.Count(),
                TotalPedidos = _db.Paquete.Count(),
                TotalRutas = _db.Ruta.Count(),
                TotalTiendas = _db.Tienda.Count(),
                Paquetes = _paqueteService.GetRecentForDashboard(10),
                RepartidoresRutas = GetRepartidoresRutas()
            };
        }

        private List<RepartidorRutaDTO> GetRepartidoresRutas()
        {
            return (from rp in _db.RutaPaquete
                    join r in _db.Ruta on rp.IdRuta equals r.IdRuta
                    join re in _db.Repartidor on r.IdRepartidor equals re.IdRepartidor
                    select new RepartidorRutaDTO
                    {
                        Repartidor = (re.Nombre + " " + re.ApellidoPaterno).ToUpper(),
                        RutaAsignada = r.IdRuta,
                        Paquete = rp.IdPaquete
                    })
                    .ToList();
        }
    }
}
