using System;
using System.ComponentModel.DataAnnotations;

namespace yotelollevo
{
    public class PaqueteMetadata
    {
        [Required(ErrorMessage = "La tienda es obligatoria.")]
        public int IdTienda { get; set; }

        [Required(ErrorMessage = "El nombre del destinatario es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
        public string NombreDestinatario { get; set; }

        [Required(ErrorMessage = "La direccion de entrega es obligatoria.")]
        [StringLength(500, ErrorMessage = "La direccion no puede superar los 500 caracteres.")]
        public string DireccionEntrega { get; set; }

        [Required(ErrorMessage = "La comuna es obligatoria.")]
        public int IdComunaEntrega { get; set; }

        [Phone(ErrorMessage = "El celular no tiene un formato valido.")]
        [StringLength(20, ErrorMessage = "El celular no puede superar los 20 caracteres.")]
        public string CelularDestinatario { get; set; }

        [StringLength(1000, ErrorMessage = "El comentario no puede superar los 1000 caracteres.")]
        public string Comentario { get; set; }

        [Required(ErrorMessage = "El estado del pedido es obligatorio.")]
        public int IdEstadoPedido { get; set; }
    }

    [MetadataType(typeof(PaqueteMetadata))]
    public partial class Paquete { }
}
