using System.Threading.Tasks;

namespace PromocionDocente.Application.Interfaces
{
    public interface IHistorialDocenteImportService
    {
        Task<string> ImportarHistorialDocenteAsync();
    }
}
