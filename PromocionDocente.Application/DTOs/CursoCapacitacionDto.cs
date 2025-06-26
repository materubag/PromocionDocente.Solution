using System;

namespace PromocionDocente.Application.DTOs
{
    public class CursoCapacitacionDto
    {
        public int Id { get; set; }

        public string CedulaDocente { get; set; } = string.Empty;

        public string Titulo { get; set; } = string.Empty;

        public string Institucion { get; set; } = string.Empty;

        public DateTime FechaCertificado { get; set; }

        public byte[]? PdfCertificado { get; set; }
    }
}
