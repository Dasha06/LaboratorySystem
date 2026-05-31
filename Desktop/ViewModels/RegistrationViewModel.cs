using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Desktop.Models;
using Desktop.Views;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class RegistrationViewModel : ViewModelBase
{
    private readonly ShellViewModel _shell;
    private string _lastName = string.Empty;
    private string _firstName = string.Empty;
    private string _middleName = string.Empty;
    private string _birthDate = string.Empty;
    private PatientDto? _selectedPatient;
    private string? _statusMessage;

    public RegistrationViewModel(ShellViewModel shell)
    {
        _shell = shell;
        FoundPatients = new ObservableCollection<PatientDto>();
        Orders = new ObservableCollection<OrderDto>();
        SearchCommand = ReactiveCommand.CreateFromTask(SearchAsync);
        CreateOrderCommand = ReactiveCommand.Create(CreateOrder,
            this.WhenAnyValue(x => x.SelectedPatient).Select((PatientDto? p) => p != null));
        AddPatientCommand = ReactiveCommand.CreateFromTask(AddPatientAsync);
    }

    public ObservableCollection<PatientDto> FoundPatients { get; }
    public ObservableCollection<OrderDto> Orders { get; }

    public string LastName { get => _lastName; set => this.RaiseAndSetIfChanged(ref _lastName, value); }
    public string FirstName { get => _firstName; set => this.RaiseAndSetIfChanged(ref _firstName, value); }
    public string MiddleName { get => _middleName; set => this.RaiseAndSetIfChanged(ref _middleName, value); }
    public string BirthDate { get => _birthDate; set => this.RaiseAndSetIfChanged(ref _birthDate, value); }

    public PatientDto? SelectedPatient
    {
        get => _selectedPatient;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedPatient, value);
            if (value != null)
                _ = LoadOrdersForPatientAsync(value.PatientId);
        }
    }

    public string? StatusMessage { get => _statusMessage; set => this.RaiseAndSetIfChanged(ref _statusMessage, value); }

    public ReactiveCommand<Unit, Unit> SearchCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateOrderCommand { get; }
    public ReactiveCommand<Unit, Unit> AddPatientCommand { get; }

    public Func<Desktop.ViewModels.CreatePatientViewModel, System.Threading.Tasks.Task<PatientDto?>>? ShowCreatePatientDialog { get; set; }

    private async Task SearchAsync()
    {
        try
        {
            var all = await AppServices.Api.GetPatientsAsync();
            FoundPatients.Clear();
            DateOnly? birthFilter = DateOnly.TryParse(BirthDate, out var d) ? d : null;

            foreach (var p in all.Where(p => MatchesPatient(p, birthFilter)))
                FoundPatients.Add(p);

            StatusMessage = $"Найдено: {FoundPatients.Count}";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private bool MatchesPatient(PatientDto p, DateOnly? birthFilter)
    {
        if (!string.IsNullOrWhiteSpace(LastName) &&
            (p.PatientLastName?.Contains(LastName, StringComparison.OrdinalIgnoreCase) != true))
            return false;
        if (!string.IsNullOrWhiteSpace(FirstName) &&
            !p.PatientFirstName.Contains(FirstName, StringComparison.OrdinalIgnoreCase))
            return false;
        if (!string.IsNullOrWhiteSpace(MiddleName) &&
            (p.PatientSecondName?.Contains(MiddleName, StringComparison.OrdinalIgnoreCase) != true))
            return false;
        if (birthFilter.HasValue && p.PatientBirthday != birthFilter)
            return false;
        return true;
    }

    private async Task LoadOrdersForPatientAsync(long patientId)
    {
        try
        {
            var orders = await AppServices.Api.GetOrdersAsync();
            Orders.Clear();
            foreach (var o in orders.Where(x => x.PatientId == patientId))
                Orders.Add(o);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private void CreateOrder()
    {
        if (SelectedPatient != null)
            _shell.OpenCreateOrder(SelectedPatient);
    }

    private async Task AddPatientAsync()
    {
        try
        {
            if (ShowCreatePatientDialog != null)
            {
                var vm = new Desktop.ViewModels.CreatePatientViewModel
                {
                    LastName = LastName,
                    FirstName = FirstName,
                    MiddleName = MiddleName,
                    BirthDate = BirthDate,
                    Gender = "Ж"
                };
                var created = await ShowCreatePatientDialog(vm);
                if (created != null)
                {
                    await AppServices.Api.CreatePatientAsync(created);
                    await SearchAsync();
                    StatusMessage = "Пациент добавлен";
                }
            }
            else
            {
                var patient = new PatientDto
                {
                    PatientFirstName = string.IsNullOrWhiteSpace(FirstName) ? "Имя" : FirstName,
                    PatientLastName = LastName,
                    PatientSecondName = MiddleName,
                    PatientGender = "Ж",
                    PatientBirthday = DateOnly.TryParse(BirthDate, out var d) ? d : null
                };
                await AppServices.Api.CreatePatientAsync(patient);
                await SearchAsync();
                StatusMessage = "Пациент добавлен";
            }
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }
}
