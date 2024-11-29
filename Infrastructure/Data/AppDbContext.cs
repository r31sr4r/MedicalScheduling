using MedicalScheduling.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed de dados para médicos
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { Id = 1, Name = "Dr. John Smith", Specialty = "Cardiology", IsActive = true },
            new Doctor { Id = 2, Name = "Dr. Sarah Connor", Specialty = "Dermatology", IsActive = true },
            new Doctor { Id = 3, Name = "Dr. Emily Carter", Specialty = "Neurology", IsActive = true },
            new Doctor { Id = 4, Name = "Dr. Michael Brown", Specialty = "Orthopedics", IsActive = true },
            new Doctor { Id = 5, Name = "Dr. Olivia Wilson", Specialty = "Pediatrics", IsActive = true }
        );
    }
}
