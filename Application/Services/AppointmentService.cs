using MedicalScheduling.Application.Interfaces;
using MedicalScheduling.Domain.Entities;

namespace MedicalScheduling.Application.Services;

public class AppointmentService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IRepository<Appointment> _appointmentRepository;

    public AppointmentService(IDoctorRepository doctorRepository, IRepository<Appointment> appointmentRepository)
    {
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
    {
        return await _doctorRepository.GetActiveDoctorsAsync();
    }

    public async Task<IEnumerable<DateTime>> GetAvailableDatesAsync(int doctorId)
    {
        var availableTimes = new List<DateTime>();
        var today = DateTime.Today;

        // Janela de horários para dias úteis (8-12, 14-18)
        var workingHours = new (TimeSpan Start, TimeSpan End)[]
        {
            (TimeSpan.FromHours(8), TimeSpan.FromHours(12)),
            (TimeSpan.FromHours(14), TimeSpan.FromHours(18))
        };

        // Buscar consultas existentes do médico
        var existingAppointments = (await _appointmentRepository.GetAllAsync())
            .Where(a => a.DoctorId == doctorId && a.DateTime.Date >= today)
            .Select(a => a.DateTime)
            .ToHashSet();

        // Gerar horários disponíveis para os próximos 7 dias
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

    public async Task ScheduleAppointment(Appointment appointment)
    {
        // Validar se o horário está disponível
        var availableDates = await GetAvailableDatesAsync(appointment.DoctorId);
        if (!availableDates.Contains(appointment.DateTime))
        {
            throw new InvalidOperationException("The selected time is not available.");
        }

        await _appointmentRepository.AddAsync(appointment);
    }
}
