namespace PromocionDocente.Application.DTOs
{
    public class ArchivoDTO
    {
        public string Titulo { get; set; } = string.Empty;
        public string Encargado { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Today;
        public string RutaArchivo { get; set; } = string.Empty;
        public byte[]? ContenidoArchivo { get; set; } // Para WebAssembly

    }
}


