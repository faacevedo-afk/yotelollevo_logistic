using System.Web.Mvc;

namespace yotelollevo.Services
{
    public interface IDropdownService
    {
        SelectList Tiendas(int? selected = null);
        SelectList Comunas(int? selected = null);
        SelectList EstadosPedido(int? selected = null);
        SelectList EstadosRuta(int? selected = null);
        SelectList RepartidoresActivos(int? selected = null);
    }
}
