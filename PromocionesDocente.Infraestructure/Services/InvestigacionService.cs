namespace PromocionDocente.Infrastructure.Services
{
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
    }

}
