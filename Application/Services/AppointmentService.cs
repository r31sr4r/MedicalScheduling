using MedicalScheduling.Application.Interfaces;
using MedicalScheduling.Domain.Entities;

namespace MedicalScheduling.Application.Services;

public class AppointmentService
{
    private readonly IDoctorRepository _doctorRepository;

    public AppointmentService(IDoctorRepository doctorRepository)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
    {
        return await _doctorRepository.GetActiveDoctorsAsync();
    }

    public async Task<IEnumerable<DateTime>> GetAvailableDatesAsync(int doctorId)
    {
        // Simular lógica de horários disponíveis para demonstração
        var today = DateTime.Today;
        return Enumerable.Range(1, 7).Select(i => today.AddDays(i)).ToList();
    }
}
