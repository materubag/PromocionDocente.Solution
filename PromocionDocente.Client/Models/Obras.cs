namespace PromocionDocente.Client.Models
{

        public class Obra
        {
        public int IdObra { get; set; }
        public string TipoObra { get; set; }
        public string Titulo { get; set; } 
        public DateTime FechaPublicacion { get; set; }
        public string? DoiUrl { get; set; }
        public string? AreaConocimiento { get; set; }
        public string? Observaciones { get; set; }
        public byte[]? PdfProduccion { get; set; }

        public bool Seleccionada { get; set; }
    }

        public class Docente_Info
        {
            public string Nombre { get; set; } = "";
            public string Cargo { get; set; } = "";
            public string Avatar { get; set; } = "";
        }
    
}
