using MedicalScheduling.Domain.Entities;

namespace MedicalScheduling.Application.Interfaces;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAppointmentsWithDetailsAsync();
    Task<IEnumerable<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
}
