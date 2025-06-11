using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.Interfaces;
using PromocionDocente.Infrastructure.Contexts;
using PromocionDocente.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//  Agrega soporte para Swagger (documentación de API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura los DbContext
builder.Services.AddDbContext<DACDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DAC")));

builder.Services.AddDbContext<PromocionDocenteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PROMOCION_DOCENTE")));

// Inyecta el servicio de importación
builder.Services.AddScoped<IObraImportService, ObraImportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
