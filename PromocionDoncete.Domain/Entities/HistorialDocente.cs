namespace PromocionDocente.Domain.Entities
{
    public class HistorialDocente
    {
        public int IdHis { get; set; }
        public string CedDoc { get; set; } = string.Empty;
        public string IdCat { get; set; } = string.Empty;
        public DateTime FecIni { get; set; }
        public DateTime? FecFin { get; set; }
    }
}
