using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Clinica.Entidades.Models;

namespace Clinica.Datos.Models;

public partial class ClinicaContext : DbContext
{
    public ClinicaContext(DbContextOptions<ClinicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Citas__3214EC07B5811525");

            entity.Property(e => e.DniPaciente).HasMaxLength(15);
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Programada");
            entity.Property(e => e.Motivo).HasMaxLength(250);
            entity.Property(e => e.NombrePaciente).HasMaxLength(120);

            entity.HasOne(d => d.Medico).WithMany(p => p.Cita)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Medicos");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Medicos__3214EC07E58EBD15");

            entity.HasIndex(e => e.Email, "UQ_Medicos_Email").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido).HasMaxLength(80);
            entity.Property(e => e.Email).HasMaxLength(120);
            entity.Property(e => e.Especialidad).HasMaxLength(80);
            entity.Property(e => e.Nombre).HasMaxLength(80);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
