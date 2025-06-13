using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Models.DITIC_Models;
using PromocionDocente.Models.PromocionDocenteModels;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportacionCursosController : ControllerBase
    {
        private readonly DiticContext _diticContext;
        private readonly PromociondocenteContext _localContext;

        public ImportacionCursosController(DiticContext diticContext, PromociondocenteContext localContext)
        {
            _diticContext = diticContext;
            _localContext = localContext;
        }

        [HttpGet("cedula/{cedula}")]
        public async Task<IActionResult> ImportarCursosPorCedula(string cedula)
        {
            // Obtener cursos de la base externa por cédula
            var cursosExternos = await _diticContext.CursosCapacitacions
                .Where(c => c.CedulaUsuario == cedula)
                .ToListAsync();

            int nuevos = 0, duplicados = 0;

            foreach (var curso in cursosExternos)
            {
                bool yaExiste = await _localContext.CursosCapacitacions.AnyAsync(c =>
                    c.CedDoc == curso.CedulaUsuario &&
                    c.NombreCurso == curso.NombreCurso &&
                    c.FechaCurso == curso.FechaCurso);

                if (!yaExiste)
                {
                    var nuevoCurso = new Models.PromocionDocenteModels.CursosCapacitacion
                    {
                        CedDoc = curso.CedulaUsuario,
                        NombreCurso = curso.NombreCurso,
                        FechaCurso = curso.FechaCurso,
                        Horas = curso.Horas,
                        PdfCurso = curso.PdfCurso
                    };

                    _localContext.CursosCapacitacions.Add(nuevoCurso);
                    nuevos++;
                }
                else
                {
                    duplicados++;
                }
            }

            await _localContext.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Importación finalizada",
                nuevos,
                duplicados,
                total = nuevos + duplicados
            });
        }
    }
}
