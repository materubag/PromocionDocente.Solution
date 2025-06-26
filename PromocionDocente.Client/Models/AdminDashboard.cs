namespace PromocionDocente.Client.Models
{
   
        // Modelo de datos para las solicitudes
        public class SolicitudDocente
        {
            public int Id { get; set; }
            public string Codigo { get; set; } = string.Empty;
            public string NombreDocente { get; set; } = string.Empty;
            public string Nivel { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
            public string TiempoEspera { get; set; } = string.Empty;
            public string Estado { get; set; } = string.Empty;
        }
    
}
