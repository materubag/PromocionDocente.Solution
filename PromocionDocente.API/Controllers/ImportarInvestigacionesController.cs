using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Models.DIDE_Models;
using PromocionDocente.Models.DITIC_Models;
using PromocionDocente.Models.PromocionDocenteModels;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportarInvestigacionesController : ControllerBase
    {
        private readonly DideContext _dideContext;
        private readonly PromocionDocenteContext _localContext;

        public ImportarInvestigacionesController(DideContext dideContext, PromocionDocenteContext localContext)
        {
            _dideContext = dideContext;
            _localContext = localContext;
        }

        [HttpGet("ced/{cedula}")]
        public async Task<IActionResult> ImportarInvestigacionesPorCedula(string cedula)
        {
            // Obtener investigaciones externas desde DIDE por cédula
            var investigacionesExternas = await _dideContext.Dides
                .Where(i => i.CedulaDocente == cedula)
                .ToListAsync();

            int nuevos = 0, duplicados = 0;

            foreach (var investigacion in investigacionesExternas)
            {
                // Verificar si ya existe en la base local
                bool yaExiste = await _localContext.Investigaciones.AnyAsync(i =>
                    i.CedDoc == investigacion.CedulaDocente &&
                    i.TituloInvestigacion == investigacion.TituloInvestigacion &&
                    i.FechaInicio == investigacion.FechaInicio);

                if (!yaExiste)
                {
                    // Crear nuevo objeto usando el modelo LOCAL
                    var nuevaInvestigacion = new Models.PromocionDocenteModels.Investigacione
                    {
                        CedDoc = investigacion.CedulaDocente,
                        TituloInvestigacion = investigacion.TituloInvestigacion,
                        DuracionMeses = investigacion.DuracionMeses,
                        FechaInicio = investigacion.FechaInicio,
                        FechaFin = investigacion.FechaFin,
                        ArchivoPdf = investigacion.ArchivoPdf,
                        TipoInvestigacion = investigacion.TipoInvestigacion,
                        CampoAplicacion = investigacion.CampoAplicacion
                    };

                    _localContext.Investigaciones.Add(nuevaInvestigacion);
                    nuevos++;
                }
                else
                {
                    duplicados++;
                }
            }

            await _localContext.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Importación de investigaciones finalizada",
                nuevos,
                duplicados,
                total = nuevos + duplicados
            });
        }
    }
}
