using Microsoft.EntityFrameworkCore;
using PromocionDocente.Domain.Interfaces;
using PromocionDocente.Models.Models;

namespace PromocionDocente.Repositories.Repositorios
{
    public class DatoDocenteHistorial : IDatoHistorial
    {
        private readonly PromocionDocenteContext _context;
        public DatoDocenteHistorial(PromocionDocenteContext context)
        {
            _context = context;
        }

        public async Task AddAsync(HistorialDocente his)
        {
            _context.HistorialDocentes.Add(his);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteHistorialAsync(string id) 
        {
            return await _context.HistorialDocentes.AnyAsync(d => d.CedDoc == id); 
        }
    }
}
