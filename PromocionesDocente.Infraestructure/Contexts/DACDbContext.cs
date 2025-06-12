using Microsoft.EntityFrameworkCore;
using PromocionDocente.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PromocionDocente.Infrastructure.Entities;

namespace PromocionDocente.Infrastructure.Contexts
{
    public class DACDbContext : DbContext
    {
        public DACDbContext(DbContextOptions<DACDbContext> options) : base(options) { }

        public DbSet<ObraDac> Obras { get; set; }
        public DbSet<EvaluacionDac> Evaluaciones { get; set; }

        public DbSet<CursoDac> Cursos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ObraDac>(entity =>
            {
                entity.ToTable("OBRAS");
                entity.HasKey(e => e.IdObra);
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

            modelBuilder.Entity<EvaluacionDac>(entity =>
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

            modelBuilder.Entity<CursoDac>(entity =>
            {
                entity.ToTable("CURSOS_CAPACITACION"); // tabla en DAC

                entity.HasKey(e => e.IdCurso);
                entity.Property(e => e.IdCurso).HasColumnName("ID_CURSO");
                entity.Property(e => e.CedulaDocente).HasColumnName("CED_DOC");
                entity.Property(e => e.NombreCurso).HasColumnName("NOMBRE_CURSO");
                entity.Property(e => e.FechaCurso).HasColumnName("FECHA_CURSO");
                entity.Property(e => e.Horas).HasColumnName("HORAS");
                entity.Property(e => e.PdfCurso).HasColumnName("PDF_CURSO");
            });

        }

    }
}

