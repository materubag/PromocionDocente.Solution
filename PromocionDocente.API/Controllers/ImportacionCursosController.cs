using Microsoft.AspNetCore.Mvc;
using PromocionDocente.API.Models;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Contexts;

namespace PromocionDocente.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportacionCursosController : ControllerBase
    {
        private readonly ICursoImportService _importService;
        private readonly PromocionDocenteDbContext _localContext; // ✅ Inyectado

        public ImportacionCursosController(ICursoImportService importService, PromocionDocenteDbContext localContext)
        {
            _importService = importService;
            _localContext = localContext; // ✅ Guardado para uso posterior
        }

        [HttpPost]
        public async Task<IActionResult> Importar()
        {
            await _importService.ImportarCursosDesdeDACAsync();
            return Ok(new { mensaje = "Cursos importados correctamente" });
        }

        [HttpPost("subir-dinamico")]
        public async Task<IActionResult> SubirCursoDesdeFormulario([FromForm] CursoFormModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            using var ms = new MemoryStream();
            await model.ArchivoPdf.CopyToAsync(ms);
            var bytesPdf = ms.ToArray();

            var curso = new Curso
            {
                CedulaDocente = model.CedulaDocente,
                NombreCurso = model.NombreCurso,
                FechaCurso = model.FechaCurso,
                Horas = model.Horas,
                PdfCurso = bytesPdf
            };

            _localContext.Cursos.Add(curso);
            await _localContext.SaveChangesAsync();

            return Ok(new { mensaje = "Curso subido correctamente", curso.IdCurso });
        }

    }
}