using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PromocionDocente.Application.DTOs;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Domain.Entities;
using PromocionDocente.Infrastructure.Contexts;

namespace PromocionDocente.Infrastructure.Services
{
    public class ArchivoService : IArchivoService
    {
        private readonly PromocionDocenteDbContext _context;
        private readonly IHostEnvironment _env;

        public async Task<bool> AgregarArchivoAsync(ArchivoDTO dto)
        {
            if (dto.ContenidoArchivo is null || dto.ContenidoArchivo.Length == 0)
                return false;

            var nombreArchivo = $"{Guid.NewGuid()}_{dto.Titulo}.pdf";
            var rutaAbsoluta = Path.Combine(_env.ContentRootPath, "archivos", nombreArchivo);
            var rutaRelativa = $"/archivos/{nombreArchivo}";

            // Guarda el PDF
            await File.WriteAllBytesAsync(rutaAbsoluta, dto.ContenidoArchivo);

            var archivo = new ArchivoDocente
            {
                Titulo = dto.Titulo,
                Encargado = dto.Encargado,
                Fecha = dto.Fecha,
                RutaArchivo = rutaRelativa
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
                var rutaAnterior = Path.Combine(_env.ContentRootPath, archivo.RutaArchivo.TrimStart('/'));
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
