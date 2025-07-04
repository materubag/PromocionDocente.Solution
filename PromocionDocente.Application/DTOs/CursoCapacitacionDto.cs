namespace PromocionDocente.Application.DTOs
{
    public class CursoCapacitacionDto
    {
        public int IdCurso { get; set; }
        public string CedulaDocente { get; set; } = string.Empty;
        public string NombreCurso { get; set; } = string.Empty;
        public DateTime FechaCurso { get; set; }
        public int Horas { get; set; }
        public byte[]? PdfCurso { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Valor fijo al crear
        public string Observacion { get; set; } = ""; // Valor fijo al crear
    }

}
