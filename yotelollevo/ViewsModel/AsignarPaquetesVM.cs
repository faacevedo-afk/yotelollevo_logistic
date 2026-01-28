using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace yotelollevo.ViewsModel
{
    public class AsignarPaquetesVM
    {
        public int IdRuta { get; set; }
        public string RutaInfo { get; set; }

        public List<int> PaquetesSeleccionados { get; set; } = new List<int>();

        public List<ItemPaquete> Pendientes { get; set; } = new List<ItemPaquete>();
        public List<ItemPaquete> YaEnRuta { get; set; } = new List<ItemPaquete>();
    }

    public class ItemPaquete
    {
        public int IdPaquete { get; set; }
        public string Tienda { get; set; }
        public string Destinatario { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
    }
}
