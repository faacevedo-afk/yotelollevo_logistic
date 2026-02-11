using System.ComponentModel.DataAnnotations;

namespace yotelollevo
{
    public class UsuarioMetadata
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(200, ErrorMessage = "El nombre no puede superar los 200 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El login es obligatorio.")]
        [StringLength(100, ErrorMessage = "El login no puede superar los 100 caracteres.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int IdRol { get; set; }
    }

    [MetadataType(typeof(UsuarioMetadata))]
    public partial class Usuario { }
}
