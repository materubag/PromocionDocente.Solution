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
        public DbSet<HistorialDocenteDac> HistorialDocente { get; set; }


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

                entity.Property(e => e.IdEvaluacion).HasColumnName("ID_EVA");
                entity.Property(e => e.FechaEvaluacion).HasColumnName("FECHA_EVALUACION");
                entity.Property(e => e.Calificacion).HasColumnName("RESULTADO");
                entity.Property(e => e.PdfEvaluacion).HasColumnName("PDF_EVALUACION");
                entity.Property(e => e.CedulaDocente).HasColumnName("CED_DOC");
                entity.Property(e => e.PeriodoEvaluacion).HasColumnName("PERIODO_EVALUADO");
                entity.Property(e => e.TipoEvaluacion).HasColumnName("TIPO_EVALUACION");
            });
            modelBuilder.Entity<HistorialDocenteDac>(entity =>
            {
                entity.ToTable("HISTORIAL_DOCENTE");

                entity.HasKey(e => e.IdHis);

                entity.Property(e => e.IdHis).HasColumnName("ID_HIS");
                entity.Property(e => e.CedDoc).HasColumnName("CED_DOC");
                entity.Property(e => e.IdCat).HasColumnName("ID_CAT");
                entity.Property(e => e.FecIni).HasColumnName("FEC_INI");
                entity.Property(e => e.FecFin).HasColumnName("FEC_FIN");
            });

        }


    }
}

