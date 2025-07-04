namespace PromocionDocente.Client.Models
{
    public class CriterioEvaluacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool EstaSeleccionado { get; set; }
        public string Color { get; set; } = "#4A90E2";
        public string Icono { get; set; } = "📊";
    }

    public class DocenteInfo
    {
        public string Nombre { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
    }
}
