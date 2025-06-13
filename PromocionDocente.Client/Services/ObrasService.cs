using PromocionDocente.Client.Models;
using System.Net.Http.Json;
using static PromocionDocente.Client.Pages.Docente.Obras;

namespace PromocionDocente.Client.Services
{
    public class ObrasService
    {
        private readonly HttpClient _httpClient;

        public ObrasService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ObraInfo>> ObtenerObrasAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ObraInfo>>("api/obras");
            return response ?? new List<ObraInfo>();
        }
    }
}
