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
        public DbSet<HistorialDocenteDac> HistorialDocente { get; set; }
        public DbSet<Docente> Docentes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Obra>(entity =>
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

        modelBuilder.Entity<Evaluacion>(entity =>
{
    entity.ToTable("EVALUACIONES");

    entity.HasKey(e => e.IdEvaluacion);
    entity.Property(e => e.IdEvaluacion).HasColumnName("ID_EVALUACION");
        entity.Property(e => e.FechaEvaluacion).HasColumnName("FECHA_EVALUACION");
        entity.Property(e => e.Calificacion).HasColumnName("RESULTADO");
        entity.Property(e => e.PdfEvaluacion).HasColumnName("PDF_EVALUACION");
        entity.Property(e => e.CedulaDocente).HasColumnName("CED_DOC");
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
            modelBuilder.Entity<Docente>(entity =>
            {
                entity.ToTable("DOCENTES");

                entity.HasKey(e => e.CedDoc);

                entity.Property(e => e.CedDoc).HasColumnName("CED_DOC");
                entity.Property(e => e.Nom1Doc).HasColumnName("NOM1_DOC");
                entity.Property(e => e.Ape1Doc).HasColumnName("APE1_DOC");
                entity.Property(e => e.TelDoc).HasColumnName("TEL_DOC");
                entity.Property(e => e.FecIng).HasColumnName("FEC_ING");
                entity.Property(e => e.FecNac).HasColumnName("FEC_NAC");
                entity.Property(e => e.IdFac).HasColumnName("ID_FAC");
                entity.Property(e => e.PdfContrato).HasColumnName("PDF_CONTRATO");
            });

        }






    }

}
