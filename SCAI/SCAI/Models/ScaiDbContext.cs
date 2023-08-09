using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SCAI.Models.Tables;

namespace SCAI;

public partial class ScaiDbContext : DbContext
{
    public ScaiDbContext()
    {
    }

    public ScaiDbContext(DbContextOptions<ScaiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
    {
        if (!optionsBuilder.IsConfigured) 
        { 
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentsId).HasName("appointments_pkey");

            entity.ToTable("appointments");

            entity.Property(e => e.AppointmentsId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("appointments_id");
            entity.Property(e => e.DoctorComment).HasColumnName("doctor_comment");
            entity.Property(e => e.FkDoctorId).HasColumnName("fk_doctor_id");
            entity.Property(e => e.FkPatientId).HasColumnName("fk_patient_id");
            entity.Property(e => e.FkResultId).HasColumnName("fk_result_id");

            entity.HasOne(d => d.FkDoctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.FkDoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointments_fk_doctor_id_fkey");

            entity.HasOne(d => d.FkPatient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.FkPatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointments_fk_patient_id_fkey");

            entity.HasOne(d => d.FkResult).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.FkResultId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointments_fk_result_id_fkey");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorsId).HasName("doctors_pkey");

            entity.ToTable("doctors");

            entity.Property(e => e.DoctorsId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("doctors_id");
            entity.Property(e => e.DoctorsFirstName)
                .HasMaxLength(50)
                .HasColumnName("doctors_first_name");
            entity.Property(e => e.DoctorsLastName)
                .HasMaxLength(50)
                .HasColumnName("doctors_last_name");
            entity.Property(e => e.DoctorsMiddleName)
                .HasMaxLength(50)
                .HasColumnName("doctors_middle_name");
            entity.Property(e => e.DoctorsPhoto)
                .HasMaxLength(255)
                .HasColumnName("doctors_photo");
            entity.Property(e => e.JobPosition)
                .HasMaxLength(50)
                .HasColumnName("job_position");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(100)
                .HasColumnName("user_password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientsId).HasName("patients_pkey");

            entity.ToTable("patients");

            entity.Property(e => e.PatientsId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("patients_id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Gender)
                .HasMaxLength(7)
                .HasColumnName("gender");
            entity.Property(e => e.PassportData)
                .HasMaxLength(20)
                .HasColumnName("passport_data");
            entity.Property(e => e.PatientsFirstName)
                .HasMaxLength(50)
                .HasColumnName("patients_first_name");
            entity.Property(e => e.PatientsLastName)
                .HasMaxLength(50)
                .HasColumnName("patients_last_name");
            entity.Property(e => e.PatientsMiddleName)
                .HasMaxLength(50)
                .HasColumnName("patients_middle_name");
            entity.Property(e => e.PatientsPhoto)
                .HasMaxLength(255)
                .HasColumnName("patients_photo");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.ResultsId).HasName("results_pkey");

            entity.ToTable("results");

            entity.Property(e => e.ResultsId)
                .UseIdentityAlwaysColumn()
                .HasColumnName("results_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(50)
                .HasColumnName("diagnosis");
            //entity.Property(e => e.FkPatientId).HasColumnName("fk_patient_id");
            entity.Property(e => e.SkinPhoto)
                .HasMaxLength(255)
                .HasColumnName("skin_photo");

            /*entity.HasOne(d => d.FkPatient).WithMany(p => p.Results)
                .HasForeignKey(d => d.FkPatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("results_fk_patient_id_fkey");*/
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
