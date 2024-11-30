using MedicalScheduling.Application.Interfaces;
using MedicalScheduling.Domain.Entities;

namespace MedicalScheduling.Application.Services;

public class AppointmentService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentService(IDoctorRepository doctorRepository, IAppointmentRepository appointmentRepository)
    {
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
    }

    // Retorna todos os médicos ativos
    public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
    {
        return await _doctorRepository.GetActiveDoctorsAsync();
    }

    // Retorna um Appointment com detalhes (Patient, Doctor)
    public async Task<Appointment?> GetAppointmentByIdAsync(int appointmentId)
    {
        var appointments = await _appointmentRepository.GetAppointmentsWithDetailsAsync();
        return appointments.FirstOrDefault(a => a.Id == appointmentId);
    }

    // Calcula datas e horários disponíveis para um médico
    public async Task<IEnumerable<DateTime>> GetAvailableDatesAsync(int doctorId)
    {
        var availableTimes = new List<DateTime>();
        var today = DateTime.Today;

        var workingHours = new (TimeSpan Start, TimeSpan End)[]
        {
            (TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
            (TimeSpan.FromHours(14), TimeSpan.FromHours(18))
        };

        // Consulta horários ocupados do médico
        var existingAppointments = (await _appointmentRepository.GetAppointmentsByDoctorIdAsync(doctorId))
            .Where(a => a.IsActive)
            .Select(a => a.DateTime)
            .ToHashSet();

        for (int day = 0; day < 7; day++)
        {
            var date = today.AddDays(day);

            // Ignorar finais de semana
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                continue;

            foreach (var (start, end) in workingHours)
            {
                for (var time = date.Add(start); time < date.Add(end); time = time.AddMinutes(30))
                {
                    if (!existingAppointments.Contains(time))
                    {
                        availableTimes.Add(time);
                    }
                }
            }
        }

        return availableTimes;
    }

    // Agenda uma nova consulta
    public async Task ScheduleAppointment(Appointment appointment)
    {
        var availableDates = await GetAvailableDatesAsync(appointment.DoctorId);
        if (!availableDates.Contains(appointment.DateTime))
        {
            throw new InvalidOperationException("The selected time is not available.");
        }

        await _appointmentRepository.AddAsync(appointment);
    }

    // Obtém todas as consultas futuras de um paciente
    public async Task<List<Appointment>> GetAppointmentsByPatientAsync(int patientId)
    {
        return (await _appointmentRepository.GetAppointmentsByPatientIdAsync(patientId))
            .Where(a => a.IsActive && a.DateTime > DateTime.Now)
            .OrderBy(a => a.DateTime)
            .ToList();
    }

    // Cancela uma consulta logicamente
    public async Task CancelAppointmentAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);
        if (appointment == null || !appointment.IsActive)
        {
            throw new InvalidOperationException("Appointment not found or already canceled.");
        }

        appointment.IsActive = false;
        await _appointmentRepository.UpdateAsync(appointment);
    }

    // Obtém consultas de um médico
    public async Task<List<Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId)
    {
        return (await _appointmentRepository.GetAppointmentsByDoctorIdAsync(doctorId))
            .OrderBy(a => a.DateTime)
            .ToList();
    }

    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        if (appointment == null)
        {
            throw new ArgumentNullException(nameof(appointment));
        }

        await _appointmentRepository.UpdateAsync(appointment);
    }

}
