using System.ComponentModel.DataAnnotations;

namespace yotelollevo
{
    public class TiendaMetadata
    {
        [Required(ErrorMessage = "El nombre de la tienda es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
        public string Nombre { get; set; }

        [EmailAddress(ErrorMessage = "El correo no tiene un formato valido.")]
        [StringLength(200, ErrorMessage = "El correo no puede superar los 200 caracteres.")]
        public string Correo { get; set; }

        [StringLength(500, ErrorMessage = "La direccion no puede superar los 500 caracteres.")]
        public string Direccion { get; set; }

        [Phone(ErrorMessage = "El telefono no tiene un formato valido.")]
        [StringLength(20, ErrorMessage = "El telefono no puede superar los 20 caracteres.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La comuna es obligatoria.")]
        public int IdComuna { get; set; }
    }

    [MetadataType(typeof(TiendaMetadata))]
    public partial class Tienda { }
}
