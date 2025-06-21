using Microsoft.EntityFrameworkCore;
using PromocionDocente.Domain.Interfaces;
using PromocionDocente.Models.PromocionDocenteModels;

namespace PromocionDocente.Repositories.Repositorios
{
    public class DatoDocenteRepositorio : IDatoDocente
    {
        private readonly PromociondocenteContext _context;

        public DatoDocenteRepositorio(PromociondocenteContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Docente log)
        {

            var facultadExiste = await _context.Facultades.AnyAsync(f => f.IdFac == log.IdFac);
            if (!facultadExiste)
            {
                throw new Exception($"La facultad con ID '{log.IdFac}' no existe.");
            }

            _context.Docentes.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteDocenteAsync(string cedula)
        {
            return await _context.Docentes.AnyAsync(d => d.CedDoc == cedula);
        }
    }

}