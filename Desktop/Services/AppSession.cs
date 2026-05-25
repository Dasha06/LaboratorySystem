using Desktop.Models;

namespace Desktop.Services;

public sealed class AppSession
{
    public WorkerDto? CurrentWorker { get; set; }
    public PatientDto? SelectedPatient { get; set; }
    public OrderDto? SelectedOrder { get; set; }
    public long? CurrentOrderId { get; set; }
    public long? SelectedTripodId { get; set; }
    public NavSection ActiveNavSection { get; set; } = NavSection.Registration;
    public NavSection? ReturnNavAfterWorksheets { get; set; }

    public void ClearAuth()
    {
        CurrentWorker = null;
        SelectedPatient = null;
        SelectedOrder = null;
        CurrentOrderId = null;
        SelectedTripodId = null;
    }
}
