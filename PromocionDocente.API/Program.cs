using Microsoft.EntityFrameworkCore;
using PromocionDocente.API.Controllers;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Application.Services;
using PromocionDocente.Domain.Interfaces;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Infrastructure.Services;

using PromocionDocente.Models.DAC_Models;
using PromocionDocente.Models.DIDE_Models;
using PromocionDocente.Models.DITIC_Models;
using PromocionDocente.Models.Models;
using PromocionDocente.Models.TTHH_Models;
using PromocionDocente.Repositories.Repositorios;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//  Agrega soporte para Swagger (documentaci�n de API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura los DbContext
//Conex�n a la base de datos de Externa (DAC)
builder.Services.AddDbContext<DACDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DAC")));
// Conexi�n a la base de datos local de Promoci�n Docente
builder.Services.AddDbContext<PromocionDocenteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PROMOCION_DOCENTE")));

// Inyecta el servicio de importacion de las Obras
builder.Services.AddScoped<IObraImportService, ObraImportService>();
// Inyecta servicio de Importacion de las evaluaciones
builder.Services.AddScoped<IEvaluacionImportService, EvaluacionImportService>();
// Inyecta servicio de importacion de historial docente
builder.Services.AddScoped<IHistorialDocenteImportService, HistorialDocenteImportService>();
// ineccion paara el calculo del Tiempo del docente
builder.Services.AddScoped<IDocenteTiempoService, DocenteTiempoService>();
builder.Services.AddScoped<IDatoDocente, DatoDocenteRepositorio>();
builder.Services.AddScoped<IUsuarioRepository, Usuario_Repository>();
builder.Services.AddScoped<AuthService>();


builder.Services.AddDbContext<PromocionDocenteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PROMOCION_DOCENTE")));
builder.Services.AddDbContext<TthhContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TTHH")));
builder.Services.AddDbContext<DacContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DAC")));
builder.Services.AddDbContext<DideContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DIDE")));
builder.Services.AddDbContext<DiticContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DITIC")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient",
        policy =>
        {
            policy.WithOrigins("https://localhost:7124")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});


var app = builder.Build();
app.UseStaticFiles();
app.UseCors("AllowAll");

// Middleware para Swagger
if (app.Environment.IsDevelopment())
{
    //  Usa Swagger en modo desarrollo
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
