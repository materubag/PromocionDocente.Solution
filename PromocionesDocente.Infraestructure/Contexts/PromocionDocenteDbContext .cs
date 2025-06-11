using Microsoft.EntityFrameworkCore;
using PromocionDocente.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromocionDocente.Infrastructure.Contexts
{
    public class PromocionDocenteDbContext : DbContext
    {
        public PromocionDocenteDbContext(DbContextOptions<PromocionDocenteDbContext> options) : base(options) { }

        public DbSet<Obra> Obras { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Obra>(entity =>
            {
                entity.ToTable("OBRAS");

                entity.HasKey(e => e.IdObra); // ⬅️ ESTO ES CLAVE

                entity.Property(e => e.IdObra).HasColumnName("ID_OBRA");
                entity.Property(e => e.CedulaDocente).HasColumnName("CED_DOC");
                entity.Property(e => e.TipoObra).HasColumnName("TIPO_OBRA");
                entity.Property(e => e.Titulo).HasColumnName("TITULO");
                entity.Property(e => e.FechaPublicacion).HasColumnName("FECHA_PUBLICACION");
                entity.Property(e => e.DoiUrl).HasColumnName("DOI_URL");
                entity.Property(e => e.AreaConocimiento).HasColumnName("AREA_CONOCIMIENTO");
                entity.Property(e => e.Observaciones).HasColumnName("OBSERVACIONES");
                entity.Property(e => e.PdfProduccion).HasColumnName("PDF_PRODUCCION");
            });

        modelBuilder.Entity<Evaluacion>(entity =>
{
    entity.ToTable("EVALUACIONES");

    entity.HasKey(e => e.IdEvaluacion);
    entity.Property(e => e.IdEvaluacion).HasColumnName("NUM_EVA");
        entity.Property(e => e.NumeroResolucion).HasColumnName("NUM_RES_EVA");
        entity.Property(e => e.FechaEvaluacion).HasColumnName("FEC_EVA");
        entity.Property(e => e.Calificacion).HasColumnName("CAL_EVA");
        entity.Property(e => e.PdfEvaluacion).HasColumnName("ARC_EVA");
        entity.Property(e => e.CedulaDocente).HasColumnName("CED_DOC");
    });


        }

    }

}
