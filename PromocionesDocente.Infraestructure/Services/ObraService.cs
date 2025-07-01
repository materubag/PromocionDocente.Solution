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
    }

}
