using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PromocionDocente.Models.DITIC_Models;

public partial class DiticContext : DbContext
{
    public DiticContext()
    {
    }

    public DiticContext(DbContextOptions<DiticContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CursosCapacitacion> CursosCapacitacions { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CursosCapacitacion>(entity =>
        {
            entity.HasKey(e => e.IdCurso).HasName("PK__CursosCa__5D3F75025B249F1C");

            entity.ToTable("CursosCapacitacion");

            entity.Property(e => e.IdCurso).HasColumnName("id_curso");
            entity.Property(e => e.CedulaUsuario)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cedula_usuario");
            entity.Property(e => e.FechaCurso).HasColumnName("fecha_curso");
            entity.Property(e => e.Horas).HasColumnName("horas");
            entity.Property(e => e.NombreCurso)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre_curso");
            entity.Property(e => e.PdfCurso).HasColumnName("pdf_curso");

            entity.HasOne(d => d.CedulaUsuarioNavigation).WithMany(p => p.CursosCapacitacions)
                .HasForeignKey(d => d.CedulaUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CursosCap__cedul__3C69FB99");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Cedula).HasName("PK__usuarios__415B7BE4910C5E5C");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Correo, "UQ__usuarios__2A586E0B7E128175").IsUnique();

            entity.Property(e => e.Cedula)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cedula");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido2");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("correo");
            entity.Property(e => e.Facultad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("facultad");
            entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");
            entity.Property(e => e.Nivel).HasColumnName("nivel");
            entity.Property(e => e.Nombre1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre1");
            entity.Property(e => e.Nombre2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre2");
            entity.Property(e => e.Rol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("rol");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
