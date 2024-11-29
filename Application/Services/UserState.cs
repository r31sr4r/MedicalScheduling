namespace MedicalScheduling.Application.Services;

public class UserState
{
    public event Action? StateChanged;

    public bool IsDoctor { get; private set; }
    public bool IsPatient { get; private set; }
    public int? PatientId { get; private set; }

    public void SetAsDoctor()
    {
        IsDoctor = true;
        IsPatient = false;
        PatientId = null; 
        NotifyStateChanged();
    }

    public void SetAsPatient(int? patientId = null)
    {
        IsPatient = true;
        IsDoctor = false;
        PatientId = patientId;
        NotifyStateChanged();
    }

    public void Reset()
    {
        IsDoctor = false;
        IsPatient = false;
        PatientId = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}
