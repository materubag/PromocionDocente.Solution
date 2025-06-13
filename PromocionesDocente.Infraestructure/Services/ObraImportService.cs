using PromocionDocente.Application.Interfaces;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace PromocionDocente.Infrastructure.Services
{
    public class ObraImportService : IObraImportService
    {
        private readonly DACDbContext _dacContext;
        private readonly PromocionDocenteDbContext _localContext;

        public ObraImportService(DACDbContext dacContext, PromocionDocenteDbContext localContext)
        {
            _dacContext = dacContext;
            _localContext = localContext;
        }

        public async Task ImportarObrasDesdeDACAsync(string cedula)
        {
            var obrasExternas = await _dacContext.Obras
                .Where(o => o.CedulaDocente == cedula)
                .ToListAsync();

            foreach (var obra in obrasExternas)
            {
                bool existe = await _localContext.Obras.AnyAsync(o => o.DoiUrl == obra.DoiUrl);

                if (!existe)
                {
                    var nuevaObra = new Obra
                    {
                        CedulaDocente = obra.CedulaDocente,
                        TipoObra = obra.TipoObra,
                        Titulo = obra.Titulo,
                        FechaPublicacion = obra.FechaPublicacion,
                        DoiUrl = obra.DoiUrl,
                        AreaConocimiento = obra.AreaConocimiento,
                        Observaciones = obra.Observaciones,
                        PdfProduccion = obra.PdfProduccion
                    };

                    _localContext.Obras.Add(nuevaObra);
                }
            }

            await _localContext.SaveChangesAsync();
        }
    }

}
