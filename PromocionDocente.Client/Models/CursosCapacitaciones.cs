namespace PromocionDocente.Client.Models
{
        public class CapacitacionInfo
        {
            public int Id { get; set; }
            public string TipoCapacitacion { get; set; } = "";
            public string Nombre { get; set; } = "";
            public string Institucion { get; set; } = "";
            public string Instructor { get; set; } = "";
            public int Horas { get; set; }
            public EstadoCapacitacion Estado { get; set; }
            public bool Seleccionado { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public string? Certificado { get; set; }
            
        }

        public enum EstadoCapacitacion
        {
            Pendiente,
            EnProgreso,
            Completado,
            Validado
        }

        public class Docente_Info2
        {
            public string Nombre { get; set; } = "";
            public string Cargo { get; set; } = "";
         
        }
    
}
