using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PromocionDocente.Models.DIDE_Models;

public partial class DideContext : DbContext
{
    public DideContext()
    {
    }

    public DideContext(DbContextOptions<DideContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dide> Dides { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dide>(entity =>
        {
            entity.HasKey(e => e.IdInvestigacion).HasName("PK__dide__A4EFA1F7BFA25057");

            entity.ToTable("dide");

            entity.Property(e => e.IdInvestigacion).HasColumnName("id_investigacion");
            entity.Property(e => e.ArchivoPdf).HasColumnName("archivo_pdf");
            entity.Property(e => e.CampoAplicacion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("campo_aplicacion");
            entity.Property(e => e.CedulaDocente)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cedula_docente");
            entity.Property(e => e.DuracionMeses).HasColumnName("duracion_meses");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.TipoInvestigacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipo_investigacion");
            entity.Property(e => e.TituloInvestigacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("titulo_investigacion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
