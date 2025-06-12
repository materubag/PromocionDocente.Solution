using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Contexts;

namespace PromocionDocente.Infrastructure.Services
{
    public class HistorialDocenteImportService : IHistorialDocenteImportService
    {
        private readonly PromocionDocenteDbContext _localContext;
        private readonly DACDbContext _dacContext;

        public HistorialDocenteImportService(PromocionDocenteDbContext localContext, DACDbContext dacContext)
        {
            _localContext = localContext;
            _dacContext = dacContext;
        }

        public async Task<string> ImportarHistorialDocenteAsync()
        {
            var registros = await _dacContext.HistorialDocente.ToListAsync();

            foreach (var registro in registros)
            {
                bool existe = await _localContext.HistorialDocente
                    .AnyAsync(r => r.IdHis == registro.IdHis);

                if (!existe)
                {
                    _localContext.HistorialDocente.Add(registro);
                }
            }

            await _localContext.SaveChangesAsync();
            return "Historial docente importado correctamente";
        }
    }
}
