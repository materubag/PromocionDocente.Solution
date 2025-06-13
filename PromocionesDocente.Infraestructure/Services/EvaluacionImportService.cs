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

        public async Task ImportarEvaluacionesDesdeDACAsync()
        {
            var evaluacionesExternas = await _dacContext.Evaluaciones.ToListAsync();

            foreach (var evaluacion in evaluacionesExternas)
            {
                bool existe = await _localContext.Evaluaciones
                    .AnyAsync(e => e.IdEvaluacion == evaluacion.IdEvaluacion);

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
                        // Sin IdEvaluacion porque es autogenerado
                    };

                    _localContext.Evaluaciones.Add(nuevaEvaluacion);
                }
            }

            await _localContext.SaveChangesAsync();
        }
    }
}
