using System.Collections.Generic;

namespace yotelollevo.Services
{
    public interface IPaqueteService
    {
        List<Paquete> GetAll(int? idTiendaFilter = null);
        Paquete GetById(int id);
        Paquete GetByIdWithIncludes(int id);
        void Create(Paquete paquete);
        void Update(Paquete paquete);
        void Delete(int id);
        List<string> GetTiendaNames();
        List<string> GetEstadoPedidoNames();
        List<yotelollevo.ViewModels.PaqueteViewModel> GetRecentForDashboard(int count = 10);
    }
}
