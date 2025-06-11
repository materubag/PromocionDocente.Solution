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


        }

    }
}

