namespace PromocionDocente.Infrastructure.Services
{
    using PromocionDocente.Application.DTOs.Update;
    using PromocionDocente.Models.Models;

    public class InvestigacionService
    {
        private readonly PromocionDocenteContext _context;

        public InvestigacionService(PromocionDocenteContext context)
        {
            _context = context;
        }

        public async Task<int> AgregarInvestigacionAsync(Investigacione investigacion)
        {
            _context.Investigaciones.Add(investigacion);
            await _context.SaveChangesAsync();
            return investigacion.IdInvestigacion;
        }
        public async Task ActualizarInvestigacionAsync(int id, InvestigacionUpdateDto dto)
        {
            var investigacion = await _context.Investigaciones.FindAsync(id);
            if (investigacion == null)
                throw new Exception("No se encontró la investigación.");

            investigacion.TituloInvestigacion = dto.TituloInvestigacion;
            investigacion.DuracionMeses = dto.DuracionMeses;
            investigacion.FechaInicio = DateOnly.FromDateTime(dto.FechaInicio);
            investigacion.FechaFin = DateOnly.FromDateTime(dto.FechaFin);
            investigacion.ArchivoPdf = dto.ArchivoPdf;
            investigacion.TipoInvestigacion = dto.TipoInvestigacion;
            investigacion.CampoAplicacion = dto.CampoAplicacion;
            investigacion.Observacion = dto.Observacion;

            await _context.SaveChangesAsync();
        }

    }

}
