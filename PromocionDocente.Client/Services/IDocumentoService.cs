using Microsoft.JSInterop;
using PromocionDocente.Client.Models;

public interface IDocumentoService
{
    Task<List<DocumentoModel>> ObtenerDocumentosAsync();
    Task ActualizarEstadoDocumentoAsync(int documentoId, string estado);
    Task<DocumentoModel?> ObtenerDocumentoPorIdAsync(int documentoId);
}

public class DocumentoService : IDocumentoService
{
    private readonly IJSRuntime _jsRuntime;

    public DocumentoService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<List<DocumentoModel>> ObtenerDocumentosAsync()
    {
        var documentos = new List<DocumentoModel>
        {
            new DocumentoModel { Id = 1, Nombre = "Hoja de Vida", Estado = "Pendiente" },
            new DocumentoModel { Id = 2, Nombre = "Títulos Académicos", Estado = "Pendiente" },
            new DocumentoModel { Id = 3, Nombre = "Certificados de Capacitación", Estado = "Pendiente" },
            new DocumentoModel { Id = 4, Nombre = "Evaluaciones Estudiantiles", Estado = "Pendiente" },
            new DocumentoModel { Id = 5, Nombre = "Publicaciones", Estado = "Pendiente" }
        };

        // Cargar estados desde localStorage
        foreach (var doc in documentos)
        {
            var estado = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", $"documento_{doc.Id}_estado");
            if (!string.IsNullOrEmpty(estado))
            {
                doc.Estado = estado;
                doc.Evaluado = estado == "Completo";
            }
        }

        return documentos;
    }

    public async Task ActualizarEstadoDocumentoAsync(int documentoId, string estado)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", $"documento_{documentoId}_estado", estado);
    }

    public async Task<DocumentoModel?> ObtenerDocumentoPorIdAsync(int documentoId)
    {
        var documentos = await ObtenerDocumentosAsync();
        return documentos.FirstOrDefault(d => d.Id == documentoId);
    }
}