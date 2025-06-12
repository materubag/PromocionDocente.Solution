namespace PromocionDocente.Client.Models
{
    public class Investigaciones
    {
        public int IdInvestigacion { get; set; }
        public string TituloInvestigacion { get; set; } 
        public int DuracionMeses { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string TipoInvestigacion { get; set; } 
        public string CampoAplicacion { get; set; }
        public string Estado { get; set; } 
        public bool Seleccionada { get; set; }
        public bool TieneArchivoPDF { get; set; }
    }

    public class Docente_Info4
    {
        public string Nombre { get; set; }
        public string Cargo { get; set; }
    }
}
