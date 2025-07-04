namespace PromocionDocente.Infrastructure.Services
{
    using PromocionDocente.Models.Models;

    public class ObraService
    {
        private readonly PromocionDocenteContext _context;

        public ObraService(PromocionDocenteContext context)
        {
            _context = context;
        }

        public async Task<int> AgregarObraAsync(Obra obra)
        {
            _context.Obras.Add(obra);
            await _context.SaveChangesAsync();
            return obra.IdObra;
        }
        public async Task ActualizarObraAsync(int id, ObraUpdateDto dto)
        {
            var obra = await _context.Obras.FindAsync(id);
            if (obra == null)
                throw new Exception("No se encontró la obra.");

            obra.Titulo = dto.Titulo;
            obra.TipoObra = dto.TipoObra;
            obra.FechaPublicacion = DateOnly.FromDateTime(dto.FechaPublicacion);
            obra.DoiUrl = dto.DoiUrl;
            obra.AreaConocimiento = dto.AreaConocimiento;
            obra.Observaciones = dto.Observaciones;
            obra.PdfProduccion = dto.PdfProduccion;

            await _context.SaveChangesAsync();
        }


    }

}
