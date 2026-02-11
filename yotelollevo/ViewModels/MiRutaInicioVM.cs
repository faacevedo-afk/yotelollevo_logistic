using System.Collections.Generic;

namespace yotelollevo.ViewModels
{
    public class MiRutaInicioVM
    {
        public Ruta RutaActiva { get; set; }
        public List<Ruta> Historial { get; set; } = new List<Ruta>();
    }
}
