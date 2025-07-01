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
    }

}
