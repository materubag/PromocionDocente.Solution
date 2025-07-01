using PromocionDocente.Models.Models;

public class CursoCapacitacionService
{
    private readonly PromocionDocenteContext _context;

    public CursoCapacitacionService(PromocionDocenteContext context)
    {
        _context = context;
    }

    public async Task<int> AgregarCursoAsync(CursosCapacitacion curso)
    {
        _context.CursosCapacitacions.Add(curso);
        await _context.SaveChangesAsync();
        return curso.IdCurso;
    }
}
