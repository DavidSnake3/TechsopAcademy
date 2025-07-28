﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechShop.Infraestructure.Models;

namespace TechShop.Infraestructure.Data;

public partial class TechAcademyContext : DbContext
{
    public TechAcademyContext(DbContextOptions<TechAcademyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CapacitacionPuestoZona> CapacitacionPuestoZona { get; set; }

    public virtual DbSet<Capacitaciones> Capacitaciones { get; set; }

    public virtual DbSet<Certificados> Certificados { get; set; }

    public virtual DbSet<HistorialCapacitacionEmpleado> HistorialCapacitacionEmpleado { get; set; }

    public virtual DbSet<MaterialComponente> MaterialComponente { get; set; }

    public virtual DbSet<OpcionesRespuesta> OpcionesRespuesta { get; set; }

    public virtual DbSet<Preguntas> Preguntas { get; set; }

    public virtual DbSet<RespuestasEmpleado> RespuestasEmpleado { get; set; }

    public virtual DbSet<ResultadosCapacitacion> ResultadosCapacitacion { get; set; }

    public virtual DbSet<SeccionCurso> SeccionCurso { get; set; }

    public virtual DbSet<VwResultadoPorEmpleado> VwResultadoPorEmpleado { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CapacitacionPuestoZona>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Capacita__3214EC07638CC5BA");

            entity.ToTable("Capacitacion_Puesto_Zona");

            entity.HasIndex(e => e.CapacitacionId, "IX_CapacitacionPuestoZona_CapacitacionId");

            entity.HasIndex(e => e.PuestoId, "IX_CapacitacionPuestoZona_PuestoId");

            entity.HasIndex(e => e.ZonaId, "IX_CapacitacionPuestoZona_ZonaId");

            entity.Property(e => e.FechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Capacitacion).WithMany(p => p.CapacitacionPuestoZona)
                .HasForeignKey(d => d.CapacitacionId)
                .HasConstraintName("FK_CapacitacionPuestoZona_Capacitacion");
        });

        modelBuilder.Entity<Capacitaciones>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Capacita__3214EC0776363370");

            entity.Property(e => e.Codigo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Dificultad).HasMaxLength(50);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(200);
        });

        modelBuilder.Entity<Certificados>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3214EC0701E9DED0");

            entity.HasIndex(e => e.ResultadoId, "IX_Certificados_ResultadoId");

            entity.Property(e => e.FechaGeneracion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RutaArchivo).HasMaxLength(500);

            entity.HasOne(d => d.Resultado).WithMany(p => p.Certificados)
                .HasForeignKey(d => d.ResultadoId)
                .HasConstraintName("FK_Certificados_Resultado");
        });

        modelBuilder.Entity<HistorialCapacitacionEmpleado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Historia__3214EC07C33D63BF");

            entity.ToTable("Historial_Capacitacion_Empleado");

            entity.HasIndex(e => e.CapacitacionId, "IX_HistorialCapacitacion_CapacitacionId");

            entity.HasIndex(e => e.EmpleadoId, "IX_HistorialCapacitacion_EmpleadoId");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaAsignacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaCompletado).HasColumnType("datetime");

            entity.HasOne(d => d.Capacitacion).WithMany(p => p.HistorialCapacitacionEmpleado)
                .HasForeignKey(d => d.CapacitacionId)
                .HasConstraintName("FK_HistorialCapacitacion_Capacitaciones");
        });

        modelBuilder.Entity<MaterialComponente>(entity =>
        {
            entity.ToTable("Material_Componente");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contenido).HasColumnName("contenido");
            entity.Property(e => e.Posicion).HasColumnName("posicion");
            entity.Property(e => e.SeccionId).HasColumnName("seccionId");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("tipo");

            entity.HasOne(d => d.Seccion).WithMany(p => p.MaterialComponente)
                .HasForeignKey(d => d.SeccionId)
                .HasConstraintName("FK_Material_Componente_Seccion_Curso");
        });

        modelBuilder.Entity<OpcionesRespuesta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Opciones__3214EC071B311EE5");

            entity.HasIndex(e => e.PreguntaId, "IX_OpcionesRespuesta_PreguntaId");

            entity.Property(e => e.TextoOpcion).HasMaxLength(500);

            entity.HasOne(d => d.Pregunta).WithMany(p => p.OpcionesRespuesta)
                .HasForeignKey(d => d.PreguntaId)
                .HasConstraintName("FK_OpcionesRespuesta_Preguntas");
        });

        modelBuilder.Entity<Preguntas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pregunta__3214EC07A47949E7");

            entity.HasIndex(e => e.CapacitacionId, "IX_Preguntas_CapacitacionId");

            entity.Property(e => e.TipoPregunta).HasMaxLength(50);

            entity.HasOne(d => d.Capacitacion).WithMany(p => p.Preguntas)
                .HasForeignKey(d => d.CapacitacionId)
                .HasConstraintName("FK_Preguntas_Capacitaciones");
        });

        modelBuilder.Entity<RespuestasEmpleado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Respuest__3214EC0709A6C438");

            entity.HasIndex(e => e.EmpleadoId, "IX_RespuestasEmpleado_EmpleadoId");

            entity.HasIndex(e => e.PreguntaId, "IX_RespuestasEmpleado_PreguntaId");

            entity.Property(e => e.FechaRespuesta)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.OpcionRespuesta).WithMany(p => p.RespuestasEmpleado)
                .HasForeignKey(d => d.OpcionRespuestaId)
                .HasConstraintName("FK_RespuestasEmpleado_OpcionesRespuesta");

            entity.HasOne(d => d.Pregunta).WithMany(p => p.RespuestasEmpleado)
                .HasForeignKey(d => d.PreguntaId)
                .HasConstraintName("FK_RespuestasEmpleado_Preguntas");
        });

        modelBuilder.Entity<ResultadosCapacitacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Resultad__3214EC07CCD14C88");

            entity.HasIndex(e => e.CapacitacionId, "IX_ResultadosCapacitacion_CapacitacionId");

            entity.HasIndex(e => e.EmpleadoId, "IX_ResultadosCapacitacion_EmpleadoId");

            entity.Property(e => e.FechaEvaluacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nota).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Capacitacion).WithMany(p => p.ResultadosCapacitacion)
                .HasForeignKey(d => d.CapacitacionId)
                .HasConstraintName("FK_ResultadosCapacitacion_Capacitaciones");
        });

        modelBuilder.Entity<SeccionCurso>(entity =>
        {
            entity.ToTable("Seccion_Curso");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CursoId).HasColumnName("cursoId");
            entity.Property(e => e.Posicion).HasColumnName("posicion");
            entity.Property(e => e.Titulo)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("titulo");

            entity.HasOne(d => d.Curso).WithMany(p => p.SeccionCurso)
                .HasForeignKey(d => d.CursoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Seccion_Curso_Capacitaciones");
        });

        modelBuilder.Entity<VwResultadoPorEmpleado>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ResultadoPorEmpleado");

            entity.Property(e => e.NotaPorcentaje).HasColumnType("decimal(5, 2)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
