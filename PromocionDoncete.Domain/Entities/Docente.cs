namespace PromocionDocente.Domain.Entities
{
    public class Docente
    {
        public string CedDoc { get; set; } = string.Empty;
        public string Nom1Doc { get; set; } = string.Empty;
        public string Ape1Doc { get; set; } = string.Empty;
        public string TelDoc { get; set; } = string.Empty;
        public DateTime FecIng { get; set; }
        public DateTime FecNac { get; set; }
        public string IdFac { get; set; } = string.Empty;
        public byte[]? PdfContrato { get; set; }
    }
}
