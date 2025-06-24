using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Infrastructure.Entities;

namespace PromocionDocente.Infrastructure.Services
{
    public class EvaluacionImportService : IEvaluacionImportService
    {
        private readonly DACDbContext _dacContext;
        private readonly PromocionDocenteDbContext _localContext;

        public EvaluacionImportService(DACDbContext dacContext, PromocionDocenteDbContext localContext)
        {
            _dacContext = dacContext;
            _localContext = localContext;
        }

        public async Task ImportarEvaluacionesDesdeDACPorCedulaAsync(string cedula)
        {
            var evaluacionesExternas = await _dacContext.Evaluaciones
                .Where(e => e.CedulaDocente == cedula)
                .ToListAsync();

            foreach (var evaluacion in evaluacionesExternas)
            {
                bool existe = await _localContext.Evaluaciones
                    .AnyAsync(e => e.CedulaDocente == evaluacion.CedulaDocente &&
                                   e.FechaEvaluacion == evaluacion.FechaEvaluacion &&
                                   e.TipoEvaluacion == evaluacion.TipoEvaluacion); // Comparación lógica

                if (!existe)
                {
                    var nuevaEvaluacion = new Evaluacion
                    {
                        CedulaDocente = evaluacion.CedulaDocente,
                        FechaEvaluacion = evaluacion.FechaEvaluacion,
                        Calificacion = evaluacion.Calificacion,
                        PdfEvaluacion = evaluacion.PdfEvaluacion,
                        PeriodoEvaluacion = evaluacion.PeriodoEvaluacion,
                        TipoEvaluacion = evaluacion.TipoEvaluacion
                    };

                    _localContext.Evaluaciones.Add(nuevaEvaluacion);
                }
            }

            await _localContext.SaveChangesAsync();
        }

    }
}
