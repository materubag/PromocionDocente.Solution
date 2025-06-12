using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace PromocionDocente.API.Models
{
    public class CursoFormModel
    {
        [Required]
        public string CedulaDocente { get; set; } = string.Empty;

        [Required]
        public string NombreCurso { get; set; } = string.Empty;

        [Required]
        public DateTime FechaCurso { get; set; }

        [Required]
        public int Horas { get; set; }

        [Required]
        public IFormFile ArchivoPdf { get; set; } = null!;
    }
}
