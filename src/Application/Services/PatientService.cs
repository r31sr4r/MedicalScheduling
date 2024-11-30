using MedicalScheduling.Application.Interfaces;
using MedicalScheduling.Domain.Entities;

namespace MedicalScheduling.Application.Services;

public class PatientService
{
    private readonly IRepository<Patient> _patientRepository;

    public PatientService(IRepository<Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task RegisterPatientAsync(Patient patient)
    {
        var existingPatient = await _patientRepository.GetAllAsync();
        if (existingPatient.Any(p => p.Email == patient.Email))
        {
            throw new InvalidOperationException("Patient with this email already exists.");
        }

        await _patientRepository.AddAsync(patient);
    }

    public async Task<Patient?> GetPatientByEmailAsync(string email)
    {
        var patients = await _patientRepository.GetAllAsync();
        return patients.FirstOrDefault(p => p.Email == email);
    }

    public async Task<Patient?> GetPatientByIdAsync(int patientId) 
        => await _patientRepository.GetByIdAsync(patientId);

    public async Task UpdatePatientAsync(Patient patient)
    {
        var existingPatient = await _patientRepository.GetByIdAsync(patient.Id);
        if (existingPatient == null)
        {
            throw new InvalidOperationException("Patient not found.");
        }

        existingPatient.Name = patient.Name;
        existingPatient.PhoneNumber = patient.PhoneNumber;
        existingPatient.Address = patient.Address;

        await _patientRepository.UpdateAsync(existingPatient);
    }



}
