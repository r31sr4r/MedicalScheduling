namespace MedicalScheduling.Domain.Entities;

public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; } = true;
}
