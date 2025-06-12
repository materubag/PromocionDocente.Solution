using Microsoft.EntityFrameworkCore;
using PromocionDocente.Application.Services;
using PromocionDocente.Domain.Interfaces;
using PromocionDocente.Infrastructure.Data;
using PromocionDocente.Infrastructure.DataPrincipal;
using PromocionDocente.Repositorio.Repository;
using Repository;

var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<DiticContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            //Se Aplica la inyeccion de dependencias con la interfaz y el usuario 
            builder.Services.AddScoped<IUsuarioRepository, Usuario_Repository>();
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddDbContext<PromociondocenteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBprincipal")));
            builder.Services.AddScoped<IDatoDocente, DatoDocenteRepositorio>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

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
