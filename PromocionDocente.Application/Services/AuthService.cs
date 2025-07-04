using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.DTOs;
using PromocionDocente.Domain.Interfaces;
using PromocionDocente.Models.TTHH_Models;
using PromocionDocente.Models.Models;


namespace PromocionDocente.Application.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IDatoDocente _dato;
        private readonly TthhContext _rrhhContext; 

        public AuthService(IUsuarioRepository repository, IDatoDocente dato, TthhContext rrhhContext)
        {
            _repository = repository;
            _dato = dato;
            _rrhhContext = rrhhContext;
        }

        public async Task<Login_Request?> LoginAsync(Login dto)
        {
            var usuario = await _repository.LoginAsync(dto.Correo, dto.Contrasena);
            if (usuario == null) return null;

            var docenteExiste = await _dato.ExisteDocenteAsync(usuario.Cedula);
            if (!docenteExiste)
            {
                // Buscar datos adicionales en RRHH usando la cédula
                var empleado = await _rrhhContext.TthhContratos
                    .FirstOrDefaultAsync(e => e.CedDoc == usuario.Cedula);

                var log = new Docente
                {
                    CedDoc = usuario.Cedula,
                    Nom1Doc = usuario.Nombre1,
                    Nom2Doc = usuario.Nombre2,
                    Ape1Doc = usuario.Apellido1,
                    Ape2Doc = usuario.Apellido2,
                    TelDoc = usuario.Telefono,
                    FecNac = new DateOnly(2020, 3, 10),
                    IdFac = usuario.Facultad,
                    NivelDocente = empleado?.NivelDocente,
                    FechaContratacion = empleado?.FechaContratacion,
                    FechaUltimoAscenso = empleado?.FechaUltimoAscenso,
                    PdfContrato = empleado?.PdfContrato,
                    EstadoContrato = empleado?.EstadoContrato
                };
                await _dato.AddAsync(log);
            }

            return new Login_Request
            {
                Cedula = usuario.Cedula,
                Nombre1 = usuario.Nombre1,
                Nombre2 = usuario.Nombre2,
                Apellido1 = usuario.Apellido1,
                Apellido2 = usuario.Apellido2,
                Facultad = usuario.Facultad,
                Telefono = usuario.Telefono,
                Correo = usuario.Correo,
                Rol = usuario.Rol
            };
        }
    }
}