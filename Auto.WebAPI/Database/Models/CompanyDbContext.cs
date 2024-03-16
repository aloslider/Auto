using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Auto.WebAPI.Database.Models;

public partial class CompanyDbContext : DbContext
{
    public CompanyDbContext(DbContextOptions<CompanyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<ConnectionType> ConnectionTypes { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Installation> Installations { get; set; }

    public virtual DbSet<PrintTask> PrintTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ConnectionType>(entity =>
        {
            entity.HasKey(e => e.Type).HasName("PK_DeviceTypes");

            entity.Property(e => e.Type).HasMaxLength(25);
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Name);

            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ConnectionType).HasMaxLength(25);

            entity.HasOne(d => d.ConnectionTypeNavigation).WithMany(p => p.Devices)
                .HasForeignKey(d => d.ConnectionType)
                .HasConstraintName("FK_Devices_ConnectionTypes");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Branch).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.BranchNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.Branch)
                .HasConstraintName("FK_Employees_Branches");
        });

        modelBuilder.Entity<Installation>(entity =>
        {
            entity.Property(e => e.Branch).HasMaxLength(50);
            entity.Property(e => e.Device).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.BranchNavigation).WithMany(p => p.Installations)
                .HasForeignKey(d => d.Branch)
                .HasConstraintName("FK_Installations_Branches");

            entity.HasOne(d => d.DeviceNavigation).WithMany(p => p.Installations)
                .HasForeignKey(d => d.Device)
                .HasConstraintName("FK_Installations_Devices");
        });

        modelBuilder.Entity<PrintTask>(entity =>
        {
            entity.HasIndex(e => e.Id, "IX_PrintTasks");

            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.PrintTasks)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_PrintTasks_PrintTasks");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
