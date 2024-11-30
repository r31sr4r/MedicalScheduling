namespace MedicalScheduling.Application.Services;

public class UserState
{
    public event Action? StateChanged;

    public bool IsDoctor { get; private set; }
    public bool IsPatient { get; private set; }
    public int? PatientId { get; private set; }
    public string? UserName { get; private set; }

    public void SetAsDoctor()
    {
        IsDoctor = true;
        IsPatient = false;
        PatientId = null;
        NotifyStateChanged();
    }

    public void SetAsPatient(int? patientId = null, string? patientName = null)
    {
        IsPatient = true;
        IsDoctor = false;
        PatientId = patientId;
        UserName = patientName;
        NotifyStateChanged();
    }

    public void Reset()
    {
        IsDoctor = false;
        IsPatient = false;
        PatientId = null;
        UserName = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}
