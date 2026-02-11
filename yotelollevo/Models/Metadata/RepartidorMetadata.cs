using System.ComponentModel.DataAnnotations;

namespace yotelollevo
{
    public class RepartidorMetadata
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio.")]
        [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string ApellidoPaterno { get; set; }

        [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "El RUT es obligatorio.")]
        [StringLength(12, ErrorMessage = "El RUT no puede superar los 12 caracteres.")]
        public string Rut { get; set; }

        [EmailAddress(ErrorMessage = "El correo no tiene un formato valido.")]
        [StringLength(200, ErrorMessage = "El correo no puede superar los 200 caracteres.")]
        public string Correo { get; set; }

        [Phone(ErrorMessage = "El celular no tiene un formato valido.")]
        [StringLength(20, ErrorMessage = "El celular no puede superar los 20 caracteres.")]
        public string Celular { get; set; }
    }

    [MetadataType(typeof(RepartidorMetadata))]
    public partial class Repartidor { }
}
