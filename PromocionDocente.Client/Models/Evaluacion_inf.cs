namespace PromocionDocente.Client.Models
{
    public class Evaluacion_inf
    {
    
   
            public int Id { get; set; }
            public string PeriodoEvaluado { get; set; }
            public string TipoEvaluacion { get; set; }
            public DateTime FechaEvaluacion { get; set; }
            public decimal Resultado { get; set; }
            public string Estado { get; set; }
            public bool Seleccionada { get; set; }
        
    }
    public class Docente_Info3
    {
        public string Nombre { get; set; } = "";
        public string Cargo { get; set; } = "";

    }
}
