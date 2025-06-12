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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PC-MATEO\\SQLEXPRESS;Database=TTHH;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TthhContrato>(entity =>
        {
            entity.HasKey(e => e.IdContrato).HasName("PK__TTHH_CON__567F8F7125A1E4F3");

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
