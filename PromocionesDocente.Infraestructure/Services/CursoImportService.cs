using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Infrastructure.Entities;

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
                }
            }

            await _localContext.SaveChangesAsync();
        }
    }
}

