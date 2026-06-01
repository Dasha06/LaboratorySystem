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
    private DateTimeOffset _takenDate = DateTime.Today;
    private string? _statusMessage;
    private long? _orderId;

    public CreateOrderViewModel(ShellViewModel shell, PatientDto patient)
    {
        _shell = shell;
        _patient = patient;
        Lpus = new ObservableCollection<LpuDto>();
        Doctors = new ObservableCollection<DoctorDto>();
        FilteredDoctors = new ObservableCollection<DoctorDto>();
        Materials = new ObservableCollection<MaterialDto>();
        Departments = new ObservableCollection<AnalysisDepartmentDto>();
        MaterialRows = new ObservableCollection<OrderMaterialRow>();
        DepartmentCategories = new ObservableCollection<AnalysisSelectionItem>();
        AnalysisChoices = new ObservableCollection<AnalysisSelectionItem>();
        SummaryRows = new ObservableCollection<OrderSummaryRow>();

        BackCommand = ReactiveCommand.Create(_shell.BackFromCreateOrder);
        SetTodayCommand = ReactiveCommand.Create(() => { TakenDate = DateTimeOffset.Parse(DateTime.Today.ToString()); });
        SetYesterdayCommand = ReactiveCommand.Create(() => { TakenDate = DateTimeOffset.Parse(DateTime.Today.AddDays(-1).ToString()); });
        
        ToggleAnalysisCommand = ReactiveCommand.Create<AnalysisSelectionItem>(ToggleAnalysisItem);
        CreateCommand = ReactiveCommand.CreateFromTask(CreateOrderAsync);
        CancelCommand = ReactiveCommand.Create(_shell.BackFromCreateOrder);
        _ = InitializeAsync();
    }

    public string PatientInfo => $"{_patient.FullName}\n\n\n Дата рождения: {_patient.PatientBirthday:dd.MM.yyyy}";

    public ObservableCollection<LpuDto> Lpus { get; }
    public ObservableCollection<DoctorDto> Doctors { get; }
    public ObservableCollection<DoctorDto> FilteredDoctors { get; }
    public ObservableCollection<MaterialDto> Materials { get; }
    public ObservableCollection<AnalysisDepartmentDto> Departments { get; }
    public ObservableCollection<OrderMaterialRow> MaterialRows { get; }
    public ObservableCollection<AnalysisSelectionItem> DepartmentCategories { get; }
    public ObservableCollection<AnalysisSelectionItem> AnalysisChoices { get; }
    public ObservableCollection<OrderSummaryRow> SummaryRows { get; }

    public LpuDto? SelectedLpu
    {
        get => _selectedLpu;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedLpu, value);
            UpdateFilteredDoctors();
            LoadAnalysesForSelectedLpu();
            this.RaisePropertyChanged(nameof(IsLpuSelected));
        }
    }

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
    public DateTimeOffset TakenDate
    {
        get => _takenDate;
        set
        {
            if (EqualityComparer<DateTimeOffset>.Default.Equals(_takenDate, value))
                return;

            this.RaiseAndSetIfChanged(ref _takenDate, value);
            for (var i = 0; i < MaterialRows.Count; i++)
            {
                var row = MaterialRows[i];
                row.TakenDate = value.ToString("dd.MM.yyyy");
                MaterialRows[i] = row;
            }
        }
    }

    public string? StatusMessage { get => _statusMessage; set => this.RaiseAndSetIfChanged(ref _statusMessage, value); }

    public bool IsLpuSelected => SelectedLpu != null;

    public ReactiveCommand<Unit, Unit> BackCommand { get; }
    public ReactiveCommand<Unit, Unit> SetTodayCommand { get; }
    public ReactiveCommand<Unit, Unit> SetYesterdayCommand { get; }
    public ReactiveCommand<AnalysisSelectionItem, Unit> ToggleAnalysisCommand { get; }
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
            if (FilteredDoctors.Count > 0) SelectedDoctor = FilteredDoctors[0];
            if (Materials.Count > 0) SelectedMaterial = Materials[0];
            if (Departments.Count > 0) SelectedDepartment = Departments[0];
            NewBarcodeId = DateTime.UtcNow.Ticks.ToString()[..12];
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private void UpdateFilteredDoctors()
    {
        FilteredDoctors.Clear();
        if (SelectedLpu == null)
        {
            SelectedDoctor = null;
            return;
        }

        var doctorsForLpu = Doctors.Where(d => d.LpuId == SelectedLpu.LpuId).ToList();
        foreach (var doctor in doctorsForLpu)
            FilteredDoctors.Add(doctor);

        if (FilteredDoctors.Count > 0)
            SelectedDoctor = FilteredDoctors[0];
        else
            SelectedDoctor = null;
    }

    private void LoadAnalysesForSelectedLpu()
    {
        AnalysisChoices.Clear();
        if (SelectedLpu == null) return;

        foreach (var analysis in _allAnalyses)
            AnalysisChoices.Add(new AnalysisSelectionItem
            {
                Code = analysis.AnalysisCodeName,
                Name = analysis.AnalysisName,
                AnalysisId = analysis.AnalysisId,
                AnalysisDepId = analysis.AnalysisDepId ?? 0
            });
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

    private void AddAnalysisItem(AnalysisSelectionItem item)
    {
        if (item == null || item.IsSelected)
        {
            if (item?.IsSelected == true)
                StatusMessage = "Анализ уже выбран";
            return;
        }

        var material = ResolveMaterialByAnalysis(item);
        if (material == null) return;

        if (!decimal.TryParse(NewBarcodeId, out var barcodeId))
        {
            StatusMessage = "Некорректный IDS";
            return;
        }

        item.IsSelected = true;
        item.BarcodeMatId = barcodeId;

        MaterialRows.Add(new OrderMaterialRow
        {
            MaterialType = material.MaterialName,
            Ids = barcodeId.ToString("0"),
            TakenDate = TakenDate.ToString("dd.MM.yyyy"),
            Comment = string.Empty,
            BarcodeMatId = barcodeId,
            MaterialId = material.MaterialId,
            AnalysisDepId = item.AnalysisDepId
        });

        SummaryRows.Add(new OrderSummaryRow
        {
            Ids = barcodeId.ToString("0"),
            Code = item.Code,
            Cipher = Cipher,
            Name = item.Name,
            AnalysisId = item.AnalysisId,
            BarcodeMatId = barcodeId
        });

        NewBarcodeId = (barcodeId + 1).ToString("0");
    }

    private void RemoveAnalysisItem(AnalysisSelectionItem item)
    {
        if (item == null || !item.IsSelected)
            return;

        var barcode = item.BarcodeMatId;
        item.IsSelected = false;
        item.BarcodeMatId = null;

        if (barcode.HasValue)
        {
            var materialRow = MaterialRows.FirstOrDefault(m => m.BarcodeMatId == barcode.Value);
            if (materialRow != null)
                MaterialRows.Remove(materialRow);

            var summaryRows = SummaryRows.Where(s => s.BarcodeMatId == barcode.Value).ToList();
            foreach (var summary in summaryRows)
                SummaryRows.Remove(summary);
        }
    }

    private void ToggleAnalysisItem(AnalysisSelectionItem item)
    {
        if (item == null)
            return;

        if (item.IsSelected)
            RemoveAnalysisItem(item);
        else
            AddAnalysisItem(item);
    }

    private MaterialDto? ResolveMaterialByAnalysis(AnalysisSelectionItem item)
    {
        if (Materials.Count == 0) return null;

        var text = $"{item.Code} {item.Name}".ToLowerInvariant();
        var match = Materials.FirstOrDefault(m => text.Contains("кров") && m.MaterialName.Contains("кров", StringComparison.OrdinalIgnoreCase))
            ?? Materials.FirstOrDefault(m => text.Contains("моч") && m.MaterialName.Contains("моч", StringComparison.OrdinalIgnoreCase))
            ?? Materials.FirstOrDefault(m => text.Contains("плазм") && m.MaterialName.Contains("плазм", StringComparison.OrdinalIgnoreCase))
            ?? Materials.FirstOrDefault(m => text.Contains("сыворот") && m.MaterialName.Contains("сыворот", StringComparison.OrdinalIgnoreCase))
            ?? Materials.FirstOrDefault(m => text.Contains("маз") && m.MaterialName.Contains("маз", StringComparison.OrdinalIgnoreCase))
            ?? Materials.FirstOrDefault(m => text.Contains("кал") && m.MaterialName.Contains("кал", StringComparison.OrdinalIgnoreCase));

        return match ?? Materials.First();
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
