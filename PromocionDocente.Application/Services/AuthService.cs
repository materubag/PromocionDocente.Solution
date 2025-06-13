using PromocionDocente.Application.DTOs;
using PromocionDocente.Domain.Interfaces;
using PromocionDocente.Infrastructure.DataPrincipal;


namespace PromocionDocente.Application.Services
{
    public class AuthService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IDatoDocente _dato;

        public AuthService(IUsuarioRepository repository, IDatoDocente dato)
        {
            _repository = repository;
            _dato = dato;
        }

        public async Task<Login_Request?> LoginAsync(Login dto)
        {
            var usuario = await _repository.LoginAsync(dto.Correo, dto.Contrasena);
            if (usuario == null) return null;

            var docenteExiste = await _dato.ExisteDocenteAsync(usuario.Cedula);
            if (!docenteExiste)
            {
                
                var log = new Docente
                {
                    CedDoc = usuario.Cedula,
                    Nom1Doc = usuario.Nombre1,
                    Nom2Doc = usuario.Nombre2,
                    Ape1Doc = usuario.Apellido1,
                    Ape2Doc = usuario.Apellido2,
                    TelDoc = usuario.Telefono,
                    FecIng = null,
                    FecNac = null,
                    IdFac = usuario.Facultad,
                    NivelDocente = null,
                    FechaContratacion = null,
                    FechaUltimoAscenso = null,
                    PdfContrato = null,
                    EstadoContrato = null
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
                Telefono=usuario.Telefono,
                Correo = usuario.Correo,
                Rol = usuario.Rol
            };
        }
    }
}