using System.Collections.ObjectModel;
using System.Reactive;
using Desktop.Models;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class CreateOrderViewModel : ViewModelBase
{
    private readonly ShellViewModel _shell;
    private readonly PatientDto _patient;
    private LpuDto? _selectedLpu;
    private DoctorDto? _selectedDoctor;
    private MaterialDto? _selectedMaterial;
    private AnalysisDepartmentDto? _selectedDepartment;
    private string _cipher = string.Empty;
    private string _department = string.Empty;
    private string _newBarcodeId = string.Empty;
    private string? _statusMessage;
    private long? _orderId;

    public CreateOrderViewModel(ShellViewModel shell, PatientDto patient)
    {
        _shell = shell;
        _patient = patient;
        Lpus = new ObservableCollection<LpuDto>();
        Doctors = new ObservableCollection<DoctorDto>();
        Materials = new ObservableCollection<MaterialDto>();
        Departments = new ObservableCollection<AnalysisDepartmentDto>();
        MaterialRows = new ObservableCollection<OrderMaterialRow>();
        DepartmentCategories = new ObservableCollection<AnalysisSelectionItem>();
        AnalysisChoices = new ObservableCollection<AnalysisSelectionItem>();
        SummaryRows = new ObservableCollection<OrderSummaryRow>();

        BackCommand = ReactiveCommand.Create(_shell.BackFromCreateOrder);
        AddMaterialCommand = ReactiveCommand.CreateFromTask(AddMaterialAsync);
        AddAnalysisCommand = ReactiveCommand.CreateFromTask(AddSelectedAnalysesAsync);
        CreateCommand = ReactiveCommand.CreateFromTask(CreateOrderAsync);
        CancelCommand = ReactiveCommand.Create(_shell.BackFromCreateOrder);
        _ = InitializeAsync();
    }

    public string PatientInfo => $"{_patient.FullName}\n\n\n Дата рождения: {_patient.PatientBirthday:dd.MM.yyyy}";

    public ObservableCollection<LpuDto> Lpus { get; }
    public ObservableCollection<DoctorDto> Doctors { get; }
    public ObservableCollection<MaterialDto> Materials { get; }
    public ObservableCollection<AnalysisDepartmentDto> Departments { get; }
    public ObservableCollection<OrderMaterialRow> MaterialRows { get; }
    public ObservableCollection<AnalysisSelectionItem> DepartmentCategories { get; }
    public ObservableCollection<AnalysisSelectionItem> AnalysisChoices { get; }
    public ObservableCollection<OrderSummaryRow> SummaryRows { get; }

    public LpuDto? SelectedLpu { get => _selectedLpu; set => this.RaiseAndSetIfChanged(ref _selectedLpu, value); }
    public DoctorDto? SelectedDoctor { get => _selectedDoctor; set => this.RaiseAndSetIfChanged(ref _selectedDoctor, value); }
    public MaterialDto? SelectedMaterial { get => _selectedMaterial; set => this.RaiseAndSetIfChanged(ref _selectedMaterial, value); }
    public AnalysisDepartmentDto? SelectedDepartment
    {
        get => _selectedDepartment;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDepartment, value);
            LoadAnalysesForDepartment();
        }
    }

    public string Cipher { get => _cipher; set => this.RaiseAndSetIfChanged(ref _cipher, value); }
    public string Department { get => _department; set => this.RaiseAndSetIfChanged(ref _department, value); }
    public string NewBarcodeId { get => _newBarcodeId; set => this.RaiseAndSetIfChanged(ref _newBarcodeId, value); }
    public string? StatusMessage { get => _statusMessage; set => this.RaiseAndSetIfChanged(ref _statusMessage, value); }

    public ReactiveCommand<Unit, Unit> BackCommand { get; }
    public ReactiveCommand<Unit, Unit> AddMaterialCommand { get; }
    public ReactiveCommand<Unit, Unit> AddAnalysisCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    private List<AnalysiseDto> _allAnalyses = [];

    private async Task InitializeAsync()
    {
        try
        {
            foreach (var l in await AppServices.Api.GetLpusAsync()) Lpus.Add(l);
            foreach (var d in await AppServices.Api.GetDoctorsAsync()) Doctors.Add(d);
            foreach (var m in await AppServices.Api.GetMaterialsAsync()) Materials.Add(m);
            foreach (var dep in await AppServices.Api.GetAnalysisDepartmentsAsync()) Departments.Add(dep);
            _allAnalyses = await AppServices.Api.GetAnalysesAsync();
            if (Lpus.Count > 0) SelectedLpu = Lpus[0];
            if (Doctors.Count > 0) SelectedDoctor = Doctors[0];
            if (Materials.Count > 0) SelectedMaterial = Materials[0];
            if (Departments.Count > 0) SelectedDepartment = Departments[0];
            NewBarcodeId = DateTime.UtcNow.Ticks.ToString()[..12];
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private void LoadAnalysesForDepartment()
    {
        AnalysisChoices.Clear();
        DepartmentCategories.Clear();
        if (SelectedDepartment == null) return;

        var depAnalyses = _allAnalyses.Where(a => a.AnalysisDepId == SelectedDepartment.AnalysisDepId).ToList();
        foreach (var a in depAnalyses.Take(8))
            DepartmentCategories.Add(new AnalysisSelectionItem
            {
                Code = a.AnalysisCodeName,
                Name = a.AnalysisName,
                AnalysisId = a.AnalysisId,
                AnalysisDepId = SelectedDepartment.AnalysisDepId
            });

        foreach (var a in depAnalyses)
            AnalysisChoices.Add(new AnalysisSelectionItem
            {
                Code = a.AnalysisCodeName,
                Name = a.AnalysisName,
                AnalysisId = a.AnalysisId,
                AnalysisDepId = SelectedDepartment.AnalysisDepId
            });
    }

    private async Task AddMaterialAsync()
    {
        if (SelectedMaterial == null || SelectedDepartment == null)
            return;
        if (!decimal.TryParse(NewBarcodeId, out var barcodeId))
        {
            StatusMessage = "Некорректный IDS";
            return;
        }

        MaterialRows.Add(new OrderMaterialRow
        {
            MaterialType = SelectedMaterial.MaterialName,
            Ids = barcodeId.ToString("0"),
            TakenDate = DateTime.Today.ToString("dd.MM.yyyy"),
            Comment = "Текст",
            BarcodeMatId = barcodeId,
            MaterialId = SelectedMaterial.MaterialId,
            AnalysisDepId = SelectedDepartment.AnalysisDepId
        });
        NewBarcodeId = (barcodeId + 1).ToString("0");
        await Task.CompletedTask;
    }

    private async Task AddSelectedAnalysesAsync()
    {
        var lastMaterial = MaterialRows.LastOrDefault();
        if (lastMaterial == null) return;

        foreach (var item in AnalysisChoices.Where(x => x.IsSelected))
        {
            SummaryRows.Add(new OrderSummaryRow
            {
                Ids = lastMaterial.Ids,
                Code = item.Code,
                Cipher = Cipher,
                Name = item.Name,
                AnalysisId = item.AnalysisId,
                BarcodeMatId = lastMaterial.BarcodeMatId
            });
        }
        await Task.CompletedTask;
    }

    private async Task CreateOrderAsync()
    {
        try
        {
            if (SelectedLpu == null)
            {
                StatusMessage = "Выберите ЛПУ";
                return;
            }

            var order = new OrderDto
            {
                PatientId = _patient.PatientId,
                LpuId = SelectedLpu.LpuId,
                DocId = SelectedDoctor?.DocId,
                OrderStatus = "Новый",
                OrderLpuDepartment = Department,
                OrderIsCountingInContract = false
            };
            await AppServices.Api.CreateOrderAsync(order);
            var orders = await AppServices.Api.GetOrdersAsync();
            _orderId = orders.Where(o => o.PatientId == _patient.PatientId).MaxBy(o => o.OrderId)?.OrderId;

            if (!_orderId.HasValue)
            {
                StatusMessage = "Не удалось определить заказ";
                return;
            }

            foreach (var m in MaterialRows)
            {
                await AppServices.Api.CreateBarcodeMaterialAsync(new BarcodeMaterialDto
                {
                    BarcodeMatId = m.BarcodeMatId,
                    OrderId = _orderId,
                    MaterialId = m.MaterialId,
                    AnalysisDepId = m.AnalysisDepId
                });
            }

            foreach (var s in SummaryRows)
            {
                await AppServices.Api.CreateBarcodeAnalysisAsync(new BarcodeAnalysiseDto
                {
                    BarcodeId = s.BarcodeMatId,
                    AnalysisId = s.AnalysisId,
                    AnalysisDepId = MaterialRows.First(m => m.BarcodeMatId == s.BarcodeMatId).AnalysisDepId
                });
            }

            StatusMessage = "Заказ создан";
            _shell.Navigate(NavSection.Registration);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }
}
