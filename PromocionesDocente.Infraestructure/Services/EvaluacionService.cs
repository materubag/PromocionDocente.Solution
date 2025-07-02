using PromocionDocente.Application.DTOs.Update;
using PromocionDocente.Models.Models;

namespace PromocionDocente.Infrastructure.Services
{
    public class EvaluacionService
    {
        private readonly PromocionDocenteContext _context;

        public EvaluacionService(PromocionDocenteContext context)
        {
            _context = context;
        }

        public async Task<int> AgregarEvaluacionAsync(Evaluacione evaluacion)
        {
            _context.Evaluaciones.Add(evaluacion);
            await _context.SaveChangesAsync();
            return evaluacion.IdEvaluacion;
        }
        public async Task ActualizarEvaluacionAsync(int id, EvaluacionUpdateDto dto)
        {
            var evaluacion = await _context.Evaluaciones.FindAsync(id);
            if (evaluacion == null)
                throw new Exception("No se encontró la evaluación.");

            evaluacion.PeriodoEvaluado = dto.PeriodoEvaluado;
            evaluacion.TipoEvaluacion = dto.TipoEvaluacion;
            evaluacion.FechaEvaluacion = DateOnly.FromDateTime(dto.FechaEvaluacion);
            evaluacion.Resultado = dto.Resultado;
            evaluacion.Observaciones = dto.Observaciones;
            evaluacion.PdfEvaluacion = dto.PdfEvaluacion;

            await _context.SaveChangesAsync();
        }

    }

}
