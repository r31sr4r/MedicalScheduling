using MedicalScheduling.Domain.Entities;

namespace MedicalScheduling.Application.Interfaces;

public interface IDoctorRepository : IRepository<Doctor>
{
    Task<IEnumerable<Doctor>> GetActiveDoctorsAsync();
}