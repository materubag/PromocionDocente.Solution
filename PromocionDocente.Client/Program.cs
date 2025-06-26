using Blazored.LocalStorage;
using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PromocionDocente.Client;
using PromocionDocente.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7088/") });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddBlazoredToast();

builder.Services.AddScoped<IDocumentoService, DocumentoService>();

await builder.Build().RunAsync();
