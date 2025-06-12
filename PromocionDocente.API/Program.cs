using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Infrastructure.Services;

using PromocionDocente.Models.DAC_Models;
using PromocionDocente.Models.DIDE_Models;
using PromocionDocente.Models.DITIC_Models;
using PromocionDocente.Models.PromocionDocenteModels;
using PromocionDocente.Models.TTHH_Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//  Agrega soporte para Swagger (documentaci�n de API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura los DbContext
builder.Services.AddDbContext<DACDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DAC")));

builder.Services.AddDbContext<PromocionDocenteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PROMOCION_DOCENTE")));

// Inyecta el servicio de importaci�n de las Obras
builder.Services.AddScoped<IObraImportService, ObraImportService>();
// Inyecta servicio de Importacion de las evaluaciones
builder.Services.AddScoped<IEvaluacionImportService, EvaluacionImportService>();
// Inyecta servicio de importaci�n de historial docente
builder.Services.AddScoped<IHistorialDocenteImportService, HistorialDocenteImportService>();
// ineccion paara el calculo del Tiempo del docente
builder.Services.AddScoped<IDocenteTiempoService, DocenteTiempoService>();

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

// Swagger
builder.Services.AddEndpointsApiExplorer(); // Habilita soporte a endpoint para Swagger
builder.Services.AddSwaggerGen();           // Agrega Swagger

var app = builder.Build();

// Middleware para Swagger
if (app.Environment.IsDevelopment())
{
    //  Usa Swagger en modo desarrollo
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
