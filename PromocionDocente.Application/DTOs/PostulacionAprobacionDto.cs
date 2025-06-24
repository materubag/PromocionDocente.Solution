using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Application.DTOs
{
    public class PostulacionAprobacionDto
    {
        [Required]
        public string CedulaDocente { get; set; }

        [Required]
        [RegularExpression("^(APROBADO|RECHAZADO)$", ErrorMessage = "El estado debe ser 'APROBADO' o 'RECHAZADO'")]
        public string Estado { get; set; }

        public string Observacion { get; set; }
    }
}
