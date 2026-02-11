using System.Collections.Generic;

namespace yotelollevo.Services
{
    public interface IEstadoService
    {
        int GetEstadoId(string nombre);
        List<int> GetEstadoIds(params string[] nombres);
        bool IsValidEstadoPedido(int idEstado);
        bool IsValidEstadoRuta(int idEstado);
    }
}
