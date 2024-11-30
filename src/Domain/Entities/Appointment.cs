namespace MedicalScheduling.Domain.Entities;

public class Appointment
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    public bool IsActive { get; set; } = true;
}
