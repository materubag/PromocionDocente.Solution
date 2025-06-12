using PromocionDocente.Infrastructure.DataPrincipal;

namespace PromocionDocente.Domain.Interfaces
{
    public interface IDatoDocente
    {
        Task AddAsync(Docente log);
        Task<bool> ExisteDocenteAsync(string id);
    }
}
