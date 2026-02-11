using System;
using System.ComponentModel.DataAnnotations;

namespace yotelollevo
{
    public class RutaMetadata
    {
        [Required(ErrorMessage = "La fecha de ruta es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaRuta { get; set; }

        [Required(ErrorMessage = "El repartidor es obligatorio.")]
        public int IdRepartidor { get; set; }

        [Required(ErrorMessage = "El estado de ruta es obligatorio.")]
        public int IdEstadoRuta { get; set; }

        [StringLength(500, ErrorMessage = "La observacion no puede superar los 500 caracteres.")]
        public string Observacion { get; set; }
    }

    [MetadataType(typeof(RutaMetadata))]
    public partial class Ruta { }
}
