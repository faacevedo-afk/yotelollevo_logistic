using System.Collections.Generic;
using yotelollevo.ViewModels;

namespace yotelollevo.Services
{
    public interface IRutaService
    {
        List<Ruta> GetAll();
        Ruta GetByIdWithDetails(int id);
        Ruta GetById(int id);
        Ruta GetByIdWithEstadoAndRepartidor(int id);
        void Create(Ruta ruta);
        void Update(Ruta ruta);
        void Delete(int id);
        bool IsRepartidorActivo(int idRepartidor);
        MiRutaInicioVM GetInicioData(int idRepartidor);
        AsignarPaquetesVM GetAsignarData(int idRuta);
        void SaveAsignacion(int idRuta, List<int> paquetesSeleccionados);
        TrabajarRutaVM GetTrabajarData(int idRuta);
        void CambiarEstadoRuta(int idRuta, string nuevoEstado);
        void MarcarEntrega(int idRutaPaquete, string nuevoEstado, string observacion);
        void Detach(Ruta ruta);
    }
}
