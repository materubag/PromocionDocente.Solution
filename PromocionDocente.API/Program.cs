using Microsoft.EntityFrameworkCore;

using PromocionDocente.Application.Interfaces;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Infrastructure.Services;

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
// Inyecta servicio de Importacion de cursos
builder.Services.AddScoped<ICursoImportService, CursoImportService>();
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