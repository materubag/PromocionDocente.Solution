using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Infrastructure.Entities;
using PromocionDocente.Infrastructure.Utils;
// Asegúrate de que esta ruta sea correcta


namespace PromocionDocente.Infrastructure.Services
{
    public class CursoImportService : ICursoImportService
    {
        private readonly DACDbContext _dacContext;
        private readonly PromocionDocenteDbContext _localContext;

        public CursoImportService(DACDbContext dacContext, PromocionDocenteDbContext localContext)
        {
            _dacContext = dacContext;
            _localContext = localContext;
        }

        public async Task ImportarCursosDesdeDACAsync()
        {
            List<Curso> cursosAGuardarPdf = new();
            var cursosExternos = await _dacContext.Cursos.ToListAsync();

            foreach (var curso in cursosExternos)
            {
                bool existe = await _localContext.Cursos.AnyAsync(c => c.IdCurso == curso.IdCurso);

                if (!existe)
                {
                    var nuevoCurso = new Curso
                    {
                        CedulaDocente = curso.CedulaDocente,
                        NombreCurso = curso.NombreCurso,
                        FechaCurso = curso.FechaCurso,
                        Horas = curso.Horas,
                        PdfCurso = curso.PdfCurso
                    };

                    _localContext.Cursos.Add(nuevoCurso);

                    cursosAGuardarPdf.Add(nuevoCurso); // ⬅ Aquí se guarda en la lista para uso posterior

                }
            }

            await _localContext.SaveChangesAsync();
            foreach (var curso in cursosAGuardarPdf)
            {
                if (curso.PdfCurso != null)
                {
                    var nombreArchivo = $"Curso_{curso.IdCurso}.pdf";
                    PdfHelper.GuardarPdfDesdeBinario(curso.PdfCurso, nombreArchivo);
                }
            }

        }
    }
}

