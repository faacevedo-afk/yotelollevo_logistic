namespace yotelollevo.Constants
{
    public static class EstadoNames
    {
        // Estados de Pedido (Paquete)
        public const string Creado = "Creado";
        public const string Asignado = "Asignado";
        public const string EnRuta = "EnRuta";
        public const string Entregado = "Entregado";
        public const string Fallido = "Fallido";
        public const string Cancelado = "Cancelado";

        // Estados de Ruta
        public const string Planificada = "Planificada";
        public const string EnCurso = "EnCurso";
        public const string Cerrada = "Cerrada";

        public static readonly string[] EstadosPedido = new[]
        {
            Creado, Asignado, EnRuta, Entregado, Fallido, Cancelado
        };

        public static readonly string[] EstadosRuta = new[]
        {
            Planificada, EnCurso, Cerrada
        };
    }
}
