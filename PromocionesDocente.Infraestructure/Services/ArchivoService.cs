using Microsoft.AspNetCore.Hosting; // ✅ Necesario
using PromocionDocente.Application.DTOs;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Contexts;

namespace PromocionDocente.Infrastructure.Services
{
    public class ArchivoService : IArchivoService
    {
        private readonly PromocionDocenteDbContext _context;
        private readonly IWebHostEnvironment _env; // ✅ Declarado

        public ArchivoService(PromocionDocenteDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<bool> AgregarArchivoAsync(ArchivoDTO dto)
        {
            var archivo = new ArchivoDocente
            {
                Titulo = dto.Titulo,
                Encargado = dto.Encargado,
                Fecha = dto.Fecha,
                RutaArchivo = dto.RutaArchivo
            };

            _context.ArchivosDocente.Add(archivo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarArchivoAsync(int id, ArchivoDTO dto)
        {
            var archivo = await _context.ArchivosDocente.FindAsync(id);
            if (archivo == null)
                return false;

            archivo.Titulo = dto.Titulo;
            archivo.Encargado = dto.Encargado;
            archivo.Fecha = dto.Fecha;

            if (!string.IsNullOrEmpty(dto.RutaArchivo) && dto.RutaArchivo != archivo.RutaArchivo)
            {
                var rutaAnterior = Path.Combine(_env.WebRootPath, archivo.RutaArchivo.TrimStart('/'));
                if (File.Exists(rutaAnterior))
                    File.Delete(rutaAnterior);

                archivo.RutaArchivo = dto.RutaArchivo;
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<ArchivoDTO?> ObtenerArchivoPorIdAsync(int id)
        {
            var archivo = await _context.ArchivosDocente.FindAsync(id);
            if (archivo == null)
                return null;

            return new ArchivoDTO
            {
                Titulo = archivo.Titulo,
                Encargado = archivo.Encargado,
                Fecha = archivo.Fecha,
                RutaArchivo = archivo.RutaArchivo
            };
        }

    }
}
