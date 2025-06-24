using System.Threading.Tasks;

namespace PromocionDocente.Application.Interfaces
{
    public interface IEvaluacionImportService
    {
        /// <summary>
        /// Importa evaluaciones desde la base de datos externa (DAC) hacia la base local (PROMOCION_DOCENTE).
        /// </summary>
        Task ImportarEvaluacionesDesdeDACPorCedulaAsync(string cedula);
    }
}
