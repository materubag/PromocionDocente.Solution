using System.Threading.Tasks;

namespace PromocionDocente.Application.Interfaces
{
    public interface IObraImportService
    {
        /// <summary>
        /// Importa obras desde la base de datos externa (DAC) hacia la base local (PROMOCION_DOCENTE).
        /// </summary>
        Task ImportarObrasDesdeDACAsync();
    }
}
