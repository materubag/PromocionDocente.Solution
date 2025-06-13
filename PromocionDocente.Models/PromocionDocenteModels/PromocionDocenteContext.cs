using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PromocionDocente.Models.PromocionDocenteModels;

public partial class PromocionDocenteContext : DbContext
{
    public PromocionDocenteContext()
    {
    }

    public PromocionDocenteContext(DbContextOptions<PromocionDocenteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<CursosCapacitacion> CursosCapacitacions { get; set; }

    public virtual DbSet<DetallePostulacion> DetallePostulacions { get; set; }

    public virtual DbSet<Docente> Docentes { get; set; }

    public virtual DbSet<Evaluacione> Evaluaciones { get; set; }

    public virtual DbSet<Facultade> Facultades { get; set; }

    public virtual DbSet<HistorialDocente> HistorialDocentes { get; set; }

    public virtual DbSet<Investigacione> Investigaciones { get; set; }

    public virtual DbSet<Obra> Obras { get; set; }

    public virtual DbSet<Postulacione> Postulaciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCat).HasName("PK__CATEGORI__2BF8FA1C0DA3989A");

            entity.ToTable("CATEGORIAS");

            entity.HasIndex(e => e.NivCat, "UQ__CATEGORI__BA0852F8E58DA39E").IsUnique();

            entity.Property(e => e.IdCat)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("ID_CAT");
            entity.Property(e => e.DesCat)
                .HasMaxLength(225)
                .IsUnicode(false)
                .HasColumnName("DES_CAT");
            entity.Property(e => e.NivCat).HasColumnName("NIV_CAT");
            entity.Property(e => e.NomCat)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NOM_CAT");
        });

        modelBuilder.Entity<CursosCapacitacion>(entity =>
        {
            entity.HasKey(e => e.IdCurso).HasName("PK__CURSOS_C__9B4AE798C446100A");

            entity.ToTable("CURSOS_CAPACITACION");

            entity.Property(e => e.IdCurso).HasColumnName("ID_CURSO");
            entity.Property(e => e.CedDoc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CED_DOC");
            entity.Property(e => e.FechaCurso).HasColumnName("FECHA_CURSO");
            entity.Property(e => e.Horas).HasColumnName("HORAS");
            entity.Property(e => e.NombreCurso)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_CURSO");
            entity.Property(e => e.PdfCurso).HasColumnName("PDF_CURSO");

            entity.HasOne(d => d.CedDocNavigation).WithMany(p => p.CursosCapacitacions)
                .HasForeignKey(d => d.CedDoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CURSOS_CA__CED_D__5165187F");
        });

        modelBuilder.Entity<DetallePostulacion>(entity =>
        {
            entity.HasKey(e => e.IdDet).HasName("PK__DETALLE___2BBEC450C1F0FB37");

            entity.ToTable("DETALLE_POSTULACION");

            entity.Property(e => e.IdDet).HasColumnName("ID_DET");
            entity.Property(e => e.Detalle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DETALLE");
            entity.Property(e => e.IdOrigen)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ID_ORIGEN");
            entity.Property(e => e.IdPos).HasColumnName("ID_POS");
            entity.Property(e => e.Rechazado)
                .HasDefaultValue(false)
                .HasColumnName("RECHAZADO");
            entity.Property(e => e.TablaOrigen)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("TABLA_ORIGEN");
            entity.Property(e => e.TipoIncumplimiento)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("TIPO_INCUMPLIMIENTO");

            entity.HasOne(d => d.IdPosNavigation).WithMany(p => p.DetallePostulacions)
                .HasForeignKey(d => d.IdPos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DETALLE_P__ID_PO__47DBAE45");
        });

        modelBuilder.Entity<Docente>(entity =>
        {
            entity.HasKey(e => e.CedDoc).HasName("PK__DOCENTES__F987F0DC983D2D47");

            entity.ToTable("DOCENTES");

            entity.Property(e => e.CedDoc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CED_DOC");
            entity.Property(e => e.Ape1Doc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("APE1_DOC");
            entity.Property(e => e.Ape2Doc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("APE2_DOC");
            entity.Property(e => e.EstadoContrato)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("ACTIVO")
                .HasColumnName("ESTADO_CONTRATO");
            entity.Property(e => e.FecIng).HasColumnName("FEC_ING");
            entity.Property(e => e.FecNac).HasColumnName("FEC_NAC");
            entity.Property(e => e.FechaContratacion).HasColumnName("FECHA_CONTRATACION");
            entity.Property(e => e.FechaUltimoAscenso).HasColumnName("FECHA_ULTIMO_ASCENSO");
            entity.Property(e => e.IdFac)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ID_FAC");
            entity.Property(e => e.NivelDocente)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("NIVEL_DOCENTE");
            entity.Property(e => e.Nom1Doc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NOM1_DOC");
            entity.Property(e => e.Nom2Doc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("NOM2_DOC");
            entity.Property(e => e.PdfContrato).HasColumnName("PDF_CONTRATO");
            entity.Property(e => e.TelDoc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TEL_DOC");

            entity.HasOne(d => d.IdFacNavigation).WithMany(p => p.Docentes)
                .HasForeignKey(d => d.IdFac)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DOCENTES__ID_FAC__3C69FB99");
        });

        modelBuilder.Entity<Evaluacione>(entity =>
        {
            entity.HasKey(e => e.IdEvaluacion).HasName("PK__EVALUACI__CE9B8DDCFCA6CCFB");

            entity.ToTable("EVALUACIONES");

            entity.Property(e => e.IdEvaluacion).HasColumnName("ID_EVALUACION");
            entity.Property(e => e.CedDoc)
                .HasMaxLength(10)
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

            entity.HasOne(d => d.CedDocNavigation).WithMany(p => p.Evaluaciones)
                .HasForeignKey(d => d.CedDoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EVALUACIO__CED_D__4BAC3F29");
        });

        modelBuilder.Entity<Facultade>(entity =>
        {
            entity.HasKey(e => e.IdFac).HasName("PK__FACULTAD__2B3AB9D80DED60F4");

            entity.ToTable("FACULTADES");

            entity.Property(e => e.IdFac)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ID_FAC");
            entity.Property(e => e.NomFac)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("NOM_FAC");
            entity.Property(e => e.UbiPreFac)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("UBI_PRE_FAC");
        });

        modelBuilder.Entity<HistorialDocente>(entity =>
        {
            entity.HasKey(e => e.IdHis).HasName("PK__HISTORIA__2E8FC4D00A839BFD");

            entity.ToTable("HISTORIAL_DOCENTE");

            entity.Property(e => e.IdHis).HasColumnName("ID_HIS");
            entity.Property(e => e.CedDoc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CED_DOC");
            entity.Property(e => e.FecFin).HasColumnName("FEC_FIN");
            entity.Property(e => e.FecIni).HasColumnName("FEC_INI");
            entity.Property(e => e.IdCat)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("ID_CAT");

            entity.HasOne(d => d.CedDocNavigation).WithMany(p => p.HistorialDocentes)
                .HasForeignKey(d => d.CedDoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HISTORIAL__CED_D__571DF1D5");

            entity.HasOne(d => d.IdCatNavigation).WithMany(p => p.HistorialDocentes)
                .HasForeignKey(d => d.IdCat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HISTORIAL__ID_CA__5812160E");
        });

        modelBuilder.Entity<Investigacione>(entity =>
        {
            entity.HasKey(e => e.IdInvestigacion).HasName("PK__INVESTIG__970DB1BC9D1BE607");

            entity.ToTable("INVESTIGACIONES");

            entity.Property(e => e.IdInvestigacion).HasColumnName("ID_INVESTIGACION");
            entity.Property(e => e.ArchivoPdf).HasColumnName("ARCHIVO_PDF");
            entity.Property(e => e.CampoAplicacion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("CAMPO_APLICACION");
            entity.Property(e => e.CedDoc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CED_DOC");
            entity.Property(e => e.DuracionMeses).HasColumnName("DURACION_MESES");
            entity.Property(e => e.FechaFin).HasColumnName("FECHA_FIN");
            entity.Property(e => e.FechaInicio).HasColumnName("FECHA_INICIO");
            entity.Property(e => e.TipoInvestigacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TIPO_INVESTIGACION");
            entity.Property(e => e.TituloInvestigacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("TITULO_INVESTIGACION");

            entity.HasOne(d => d.CedDocNavigation).WithMany(p => p.Investigaciones)
                .HasForeignKey(d => d.CedDoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__INVESTIGA__CED_D__5441852A");
        });

        modelBuilder.Entity<Obra>(entity =>
        {
            entity.HasKey(e => e.IdObra).HasName("PK__OBRAS__4D68E95A04E5880D");

            entity.ToTable("OBRAS");

            entity.Property(e => e.IdObra).HasColumnName("ID_OBRA");
            entity.Property(e => e.AreaConocimiento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("AREA_CONOCIMIENTO");
            entity.Property(e => e.CedDoc)
                .HasMaxLength(10)
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

            entity.HasOne(d => d.CedDocNavigation).WithMany(p => p.Obras)
                .HasForeignKey(d => d.CedDoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OBRAS__CED_DOC__4E88ABD4");
        });

        modelBuilder.Entity<Postulacione>(entity =>
        {
            entity.HasKey(e => e.IdPos).HasName("PK__POSTULAC__20AFD1976E491C31");

            entity.ToTable("POSTULACIONES");

            entity.Property(e => e.IdPos).HasColumnName("ID_POS");
            entity.Property(e => e.CedDoc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("CED_DOC");
            entity.Property(e => e.EstPos)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("EST_POS");
            entity.Property(e => e.FecPos).HasColumnName("FEC_POS");
            entity.Property(e => e.IdCat)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("ID_CAT");
            entity.Property(e => e.ObsPos)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("OBS_POS");

            entity.HasOne(d => d.CedDocNavigation).WithMany(p => p.Postulaciones)
                .HasForeignKey(d => d.CedDoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POSTULACI__CED_D__4316F928");

            entity.HasOne(d => d.IdCatNavigation).WithMany(p => p.Postulaciones)
                .HasForeignKey(d => d.IdCat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__POSTULACI__ID_CA__440B1D61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
