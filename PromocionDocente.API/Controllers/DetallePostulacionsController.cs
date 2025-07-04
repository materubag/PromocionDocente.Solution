using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.DTOs;
using PromocionDocente.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocionDocente.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePostulacionsController : ControllerBase
    {
        private readonly PromocionDocenteContext _context;

        public DetallePostulacionsController(PromocionDocenteContext context)
        {
            _context = context;
        }

        // GET: api/DetallePostulacions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePostulacion>>> GetDetallePostulacions()
        {
            return await _context.DetallePostulacions.ToListAsync();
        }

        // GET: api/DetallePostulacions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetallePostulacion>> GetDetallePostulacion(int id)
        {
            var detallePostulacion = await _context.DetallePostulacions.FindAsync(id);

            if (detallePostulacion == null)
            {
                return NotFound();
            }

            return detallePostulacion;
        }

        

        // POST: api/DetallePostulacions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DetallePostulacion>> PostDetallePostulacion(DetallePostulacion detallePostulacion)
        {
            _context.DetallePostulacions.Add(detallePostulacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetallePostulacion", new { id = detallePostulacion.IdDet }, detallePostulacion);
        }

        private bool DetallePostulacionExists(int id)
        {
            return _context.DetallePostulacions.Any(e => e.IdDet == id);
        }


        // GET: api/DetallePostulacions/rechazados/{cedula}
        [HttpGet("rechazados/{cedula}")]
        public async Task<ActionResult<ReporteRechazadosDto>> GetReporteRechazados(string cedula)
        {
            try
            {
                // 1. Verificar que el docente existe
                var docente = await _context.Docentes
                    .Where(d => d.CedDoc == cedula)
                    .Select(d => new { d.Nom1Doc })
                    .FirstOrDefaultAsync();

                if (docente == null)
                {
                    return NotFound($"No se encontró el docente con cédula {cedula}");
                }

                // 2. Obtener la última fecha del historial (si existe)
                var ultimaFecha = await _context.HistorialDocentes
                    .Where(h => h.CedDoc == cedula)
                    .OrderByDescending(h => h.FecIni)
                    .Select(h => h.FecIni)
                    .FirstOrDefaultAsync();

                // 3. Obtener el revisor de la última postulación del docente
                var ultimaPostulacion = await _context.Postulaciones
                    .Where(p => p.CedDoc == cedula)
                    .OrderByDescending(p => p.FecPos)
                    .Select(p => new { p.IdPos, p.Revisor })
                    .FirstOrDefaultAsync();

                string revisor = ultimaPostulacion?.Revisor ?? "Sistema";
                int idPostulacion = ultimaPostulacion?.IdPos ?? 0;

                // 4. Obtener elementos rechazados
                var obrasRechazadas = await _context.Obras
                    .Where(o => o.CedDoc == cedula && o.Estado == "RECHAZADO")
                    .Select(o => new ObraRechazadaDto
                    {
                        IdObra = o.IdObra,
                        Titulo = o.Titulo,
                        TipoObra = o.TipoObra,
                        FechaPublicacion = o.FechaPublicacion,
                        Observacion = o.Observacion
                    })
                    .ToListAsync();

                var investigacionesRechazadas = await _context.Investigaciones
                    .Where(i => i.CedDoc == cedula && i.Estado == "RECHAZADO")
                    .Select(i => new InvestigacionRechazadaDto
                    {
                        IdInvestigacion = i.IdInvestigacion,
                        TituloInvestigacion = i.TituloInvestigacion,
                        FechaInicio = i.FechaInicio,
                        FechaFin = i.FechaFin,
                        Observacion = i.Observacion
                    })
                    .ToListAsync();

                var cursosRechazados = await _context.CursosCapacitacions
                    .Where(c => c.CedDoc == cedula && c.Estado == "RECHAZADO")
                    .Select(c => new CursoRechazadoDto
                    {
                        IdCurso = c.IdCurso,
                        NombreCurso = c.NombreCurso,
                        FechaCurso = c.FechaCurso,
                        Horas = c.Horas,
                        Observacion = c.Observacion
                    })
                    .ToListAsync();

                var evaluacionesRechazadas = await _context.Evaluaciones
                    .Where(e => e.CedDoc == cedula && e.Estado == "RECHAZADO")
                    .Select(e => new EvaluacionRechazadaDto
                    {
                        IdEvaluacion = e.IdEvaluacion,
                        TipoEvaluacion = e.TipoEvaluacion,
                        PeriodoEvaluado = e.PeriodoEvaluado,
                        Resultado = e.Resultado,
                        Observacion = e.Observacion
                    })
                    .ToListAsync();

                // 5. Crear el reporte
                var reporte = new ReporteRechazadosDto
                {
                    CedulaDocente = cedula,
                    NombreDocente = docente.Nom1Doc,
                    UltimaFechaHistorial = ultimaFecha,
                    ObrasRechazadas = obrasRechazadas,
                    InvestigacionesRechazadas = investigacionesRechazadas,
                    CursosRechazados = cursosRechazados,
                    EvaluacionesRechazadas = evaluacionesRechazadas,
                    TotalRechazados = obrasRechazadas.Count + investigacionesRechazadas.Count +
                                     cursosRechazados.Count + evaluacionesRechazadas.Count,
                    FechaGeneracion = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                    UsuarioGeneracion = revisor // Usar el revisor de la postulación en lugar del usuario actual
                };

                return reporte;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al generar el reporte: {ex.Message}");
            }
        }
    }
}
