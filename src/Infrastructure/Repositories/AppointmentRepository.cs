using MedicalScheduling.Application.Interfaces;
using MedicalScheduling.Domain.Entities;
using MedicalScheduling.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Repositories;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsWithDetailsAsync()
    {
        return await _context.Appointments
            .Include(a => a.Patient) // Carregar o paciente
            .Include(a => a.Doctor)  // Carregar o médico
            .Where(a => a.IsActive)  // Apenas consultas ativas
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.IsActive && a.DoctorId == doctorId)
            .OrderBy(a => a.DateTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Where(a => a.IsActive && a.PatientId == patientId)
            .OrderBy(a => a.DateTime)
            .ToListAsync();
    }
}
