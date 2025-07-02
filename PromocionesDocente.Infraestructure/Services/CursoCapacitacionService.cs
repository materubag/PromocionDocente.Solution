using PromocionDocente.Application.DTOs.Update;
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
    public async Task ActualizarCursoAsync(int id, CursoCapacitacionUpdateDto dto)
    {
        var curso = await _context.CursosCapacitacions.FindAsync(id);
        if (curso == null)
            throw new Exception("No se encontró el curso.");

        curso.NombreCurso = dto.NombreCurso;
        curso.FechaCurso = DateOnly.FromDateTime(dto.FechaCurso);
        curso.Horas = dto.Horas;
        curso.PdfCurso = dto.PdfCurso;
        curso.Observacion = dto.Observacion;

        await _context.SaveChangesAsync();
    }

}
