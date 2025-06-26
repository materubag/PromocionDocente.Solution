using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PromocionDocente.Models.TTHH_Models;

public partial class TthhContext : DbContext
{
    public TthhContext()
    {
    }

    public TthhContext(DbContextOptions<TthhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TthhContrato> TthhContratos { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TthhContrato>(entity =>
        {
            entity.HasKey(e => e.IdContrato).HasName("PK__TTHH_CON__567F8F711948495A");

            entity.ToTable("TTHH_CONTRATO");

            entity.Property(e => e.IdContrato).HasColumnName("ID_CONTRATO");
            entity.Property(e => e.CedDoc)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("CED_DOC");
            entity.Property(e => e.EstadoContrato)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("ACTIVO")
                .HasColumnName("ESTADO_CONTRATO");
            entity.Property(e => e.FechaContratacion).HasColumnName("FECHA_CONTRATACION");
            entity.Property(e => e.FechaUltimoAscenso).HasColumnName("FECHA_ULTIMO_ASCENSO");
            entity.Property(e => e.NivelDocente)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("NIVEL_DOCENTE");
            entity.Property(e => e.NombreDocente)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_DOCENTE");
            entity.Property(e => e.PdfContrato).HasColumnName("PDF_CONTRATO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
