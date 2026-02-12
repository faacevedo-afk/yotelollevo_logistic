using System;
using System.Collections.Generic;

namespace yotelollevo.ViewModels
{
    public class TrabajarRutaVM
    {
        public int IdRuta { get; set; }
        public DateTime FechaRuta { get; set; }
        public string EstadoRuta { get; set; }
        public bool PuedeIniciar { get; set; }
        public bool PuedeCerrar { get; set; }
        public List<TrabajoPaqueteItem> Paquetes { get; set; }
    }

    public class TrabajoPaqueteItem
    {
        public int IdRutaPaquete { get; set; }
        public int IdPaquete { get; set; }
        public int OrdenEntrega { get; set; }
        public string Destinatario { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public string Tienda { get; set; }
        public string EstadoEntrega { get; set; }
        public string Observacion { get; set; }
        public DateTime? HoraEvento { get; set; }
        public bool YaFinalizado { get; set; }
    }
}
