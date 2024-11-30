namespace MedicalScheduling.Domain.Entities;

public class Doctor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Specialty { get; set; }
    public bool IsActive { get; set; } = true;
}
