using System.Collections.Generic;

namespace yotelollevo.ViewModels
{
    public class HomeIndexViewModel
    {
        public int TotalPaquetes { get; set; }
        public int TotalPedidos { get; set; }
        public int TotalRutas { get; set; }
        public int TotalTiendas { get; set; }

        public List<PaqueteViewModel> Paquetes { get; set; }
        public List<RepartidorRutaDTO> RepartidoresRutas { get; set; }
    }

    public class PaqueteViewModel
    {
        public int IdPaquete { get; set; }
        public string Destinatario { get; set; }
        public string TiendaNombre { get; set; }
        public string DireccionEntrega { get; set; }
        public string EstadoNombre { get; set; }
        public string EstadoColor { get; set; }
        public string RutaCodigo { get; set; }
        public string RutaNombre { get; set; }
        public string RepartidorNombre { get; set; }
        public string RepartidorTelefono { get; set; }
        public string CodigoProducto { get; set; }
    }

    public class RepartidorRutaDTO
    {
        public string Repartidor { get; set; }
        public int RutaAsignada { get; set; }
        public int Paquete { get; set; }
    }
}
