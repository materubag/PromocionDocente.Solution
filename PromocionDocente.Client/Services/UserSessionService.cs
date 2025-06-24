using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace PromocionDocente.Client.Services
{
    public class UserSessionService
    {
        private readonly ILocalStorageService _localStorage;
        public LoginResponse? Usuario { get; private set; }

        public UserSessionService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task CargarUsuarioAsync()
        {
            Usuario = await _localStorage.GetItemAsync<LoginResponse>("usuario");
        }

       
        public async Task EliminarUsuarioAsync()
        {
          
            Usuario = null;
            await Task.CompletedTask;
        }
    }

    public class LoginResponse
    {
        public string Cedula { get; set; } = string.Empty;
        public string Nombre1 { get; set; } = string.Empty;
        public string Apellido1 { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}