using MedicalScheduling.Application.Interfaces;
using MedicalScheduling.Domain.Entities;
using MedicalScheduling.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MedicalScheduling.Infrastructure.Repositories;

public class DoctorRepository : Repository<Doctor>, IDoctorRepository
{
    private readonly AppDbContext _context;

    public DoctorRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Doctor>> GetActiveDoctorsAsync()
    {
        return await _context.Doctors.Where(d => d.IsActive).ToListAsync();
    }
}
