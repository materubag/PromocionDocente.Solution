using PromocionDocente.Models.Models;

namespace PromocionDocente.Domain.Interfaces
{
   public interface IDatoHistorial
    {
        Task AddAsync(HistorialDocente log);
        Task<bool> ExisteHistorialAsync(string id);
    }
}
