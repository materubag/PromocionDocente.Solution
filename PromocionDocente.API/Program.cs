using Microsoft.EntityFrameworkCore;
using PromocionDocente.Models.PromocionDocenteModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<PromocionDocenteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PROMOCION_DOCENTE")));
builder.Services.AddDbContext<PromocionDocenteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TTHH")));
builder.Services.AddDbContext<PromocionDocenteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DAC")));

// Swagger
builder.Services.AddEndpointsApiExplorer(); // Habilita soporte a endpoint para Swagger
builder.Services.AddSwaggerGen();           // Agrega Swagger

var app = builder.Build();

// Middleware para Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Abre la interfaz web de Swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();