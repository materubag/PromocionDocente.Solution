using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PromocionDocente.Models.DAC_Models;

public partial class DacContext : DbContext
{
    public DacContext()
    {
    }

    public DacContext(DbContextOptions<DacContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DacEvaluacion> DacEvaluacions { get; set; }

    public virtual DbSet<DacObra> DacObras { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DacEvaluacion>(entity =>
        {
            entity.HasKey(e => e.IdEvaluacion).HasName("PK__DAC_EVAL__CE9B8DDC7C5BAC44");

            entity.ToTable("DAC_EVALUACION");

            entity.Property(e => e.IdEvaluacion).HasColumnName("ID_EVALUACION");
            entity.Property(e => e.CedDoc)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CED_DOC");
            entity.Property(e => e.FechaEvaluacion).HasColumnName("FECHA_EVALUACION");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("OBSERVACIONES");
            entity.Property(e => e.PdfEvaluacion).HasColumnName("PDF_EVALUACION");
            entity.Property(e => e.PeriodoEvaluado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PERIODO_EVALUADO");
            entity.Property(e => e.Resultado)
                .HasColumnType("numeric(5, 2)")
                .HasColumnName("RESULTADO");
            entity.Property(e => e.TipoEvaluacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIPO_EVALUACION");
        });

        modelBuilder.Entity<DacObra>(entity =>
        {
            entity.HasKey(e => e.IdObra).HasName("PK__DAC_OBRA__4D68E95A8CF45ACA");

            entity.ToTable("DAC_OBRAS");

            entity.Property(e => e.IdObra).HasColumnName("ID_OBRA");
            entity.Property(e => e.AreaConocimiento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("AREA_CONOCIMIENTO");
            entity.Property(e => e.CedDoc)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CED_DOC");
            entity.Property(e => e.DoiUrl)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("DOI_URL");
            entity.Property(e => e.FechaPublicacion).HasColumnName("FECHA_PUBLICACION");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("OBSERVACIONES");
            entity.Property(e => e.PdfProduccion).HasColumnName("PDF_PRODUCCION");
            entity.Property(e => e.TipoObra)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TIPO_OBRA");
            entity.Property(e => e.Titulo)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("TITULO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
