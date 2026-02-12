using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using yotelollevo.Constants;
using yotelollevo.ViewModels;

namespace yotelollevo.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly LogisticaDBEntities _db;
        private readonly IPaqueteService _paqueteService;
        private readonly IEstadoService _estadoService;

        public DashboardService(LogisticaDBEntities db, IPaqueteService paqueteService, IEstadoService estadoService)
        {
            _db = db;
            _paqueteService = paqueteService;
            _estadoService = estadoService;
        }

        public HomeIndexViewModel GetDashboardData()
        {
            int idCreado = _estadoService.GetEstadoId(EstadoNames.Creado);
            int idEntregado = _estadoService.GetEstadoId(EstadoNames.Entregado);
            var idsRutaActiva = _estadoService.GetEstadoIds(EstadoNames.Planificada, EstadoNames.EnCurso);

            return new HomeIndexViewModel
            {
                TotalPaquetes = _db.Paquete.Count(),
                EnBodega = _db.Paquete.Count(p => p.IdEstadoPedido == idCreado),
                EnRuta = _db.Ruta.Count(r => idsRutaActiva.Contains(r.IdEstadoRuta)),
                Entregados = _db.Paquete.Count(p => p.IdEstadoPedido == idEntregado),
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
