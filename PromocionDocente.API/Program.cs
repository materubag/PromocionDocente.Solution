
using Microsoft.EntityFrameworkCore;
using PromocionDocente.Models.DAC_Models;
using PromocionDocente.Models.DIDE_Models;
using PromocionDocente.Models.DITIC_Models;
using PromocionDocente.Models.PromocionDocenteModels;
using PromocionDocente.Models.TTHH_Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

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
    app.UseSwagger();
    app.UseSwaggerUI(); // Abre la interfaz web de Swagger

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
