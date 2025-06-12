using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Contexts;

namespace PromocionDocente.Infrastructure.Services
{
    public class DocenteTiempoService : IDocenteTiempoService
    {
        private readonly PromocionDocenteDbContext _context;

        public DocenteTiempoService(PromocionDocenteDbContext context)
        {
            _context = context;
        }

        public async Task<DocenteTiempo?> ObtenerTiempoDocenteAsync(string cedula, DateTime? fechaCorte = null)
        {
            var fechaHasta = fechaCorte ?? DateTime.Now;

            var docente = await _context.Docentes.FirstOrDefaultAsync(d => d.CedDoc == cedula);  // ✅
            if (docente == null) return null;

            TimeSpan total = fechaHasta - docente.FecIng;  // ✅ parte actual

            var historial = await _context.HistorialDocente
                .Where(h => h.CedDoc == cedula)
                .ToListAsync();

            // sumar historial pasado
            foreach (var h in historial)
            {
                var ini = h.FecIni;
                var fin = h.FecFin ?? fechaHasta;

                if (ini < fechaHasta)
                {
                    if (fin > fechaHasta) fin = fechaHasta;
                    total += fin - ini;
                }
            }

            int totalDias = (int)total.TotalDays;
            int anios = totalDias / 365;
            int meses = (totalDias % 365) / 30;
            int dias = (totalDias % 365) % 30;

            return new DocenteTiempo
            {
                Cedula = cedula,
                Anios = anios,
                Meses = meses,
                Dias = dias
            };
        }
    }
}
