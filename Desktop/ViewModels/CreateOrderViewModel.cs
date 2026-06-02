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
    private readonly OrderDto? _editingOrder;
    private LpuDto? _selectedLpu;
    private DoctorDto? _selectedDoctor;
    private MaterialDto? _selectedMaterial;
    private AnalysisDepartmentDto? _selectedDepartment;
    private string _cipher = string.Empty;
    private string _department = string.Empty;
    private string _newBarcodeId = string.Empty;
    private DateTimeOffset _takenDate = DateTime.Today;
    private string _orderStatus = "Новый";
    private bool _orderIsCountingInContract;
    private string? _statusMessage;
    private long? _orderId;
    private AnalysisSelectionItem? _selectedAnalysis;
    private List<ContractAnalysisDto> _selectedLpuContractAnalyses = new List<ContractAnalysisDto>();

    public CreateOrderViewModel(ShellViewModel shell, PatientDto patient, OrderDto? editingOrder = null)
    {
        _shell = shell;
        _patient = patient;
        _editingOrder = editingOrder;
        OrderStatus = editingOrder?.OrderStatus ?? "Новый";
        OrderIsCountingInContract = editingOrder?.OrderIsCountingInContract ?? false;
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
        CreateCommand = ReactiveCommand.CreateFromTask(SubmitOrderAsync);
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

    public AnalysisSelectionItem? SelectedAnalysis
    {
        get => _selectedAnalysis;
        set
        {
            if (EqualityComparer<AnalysisSelectionItem?>.Default.Equals(_selectedAnalysis, value))
                return;

            this.RaiseAndSetIfChanged(ref _selectedAnalysis, value);

            if (value == null)
                return;

            if (value.IsSelected)
            {
                RemoveAnalysisItem(value);
            }
            else
            {
                AddAnalysisItem(value);
            }

            _selectedAnalysis = null;
            this.RaisePropertyChanged(nameof(SelectedAnalysis));
        }
    }

    public LpuDto? SelectedLpu
    {
        get => _selectedLpu;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedLpu, value);
            UpdateFilteredDoctors();
            _ = LoadAnalysesForSelectedLpuAsync();
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

    public string OrderStatus { get => _orderStatus; set => this.RaiseAndSetIfChanged(ref _orderStatus, value); }
    public bool OrderIsCountingInContract { get => _orderIsCountingInContract; set => this.RaiseAndSetIfChanged(ref _orderIsCountingInContract, value); }
    public bool IsEditMode => _editingOrder != null;
    public string PageTitle => IsEditMode ? "Редактирование заказа" : "Создание заказа";
    public string SubmitButtonText => IsEditMode ? "Сохранить" : "Создать";

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
                row.TakenDate = value.ToString();
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
            if (Lpus.Count > 0)
            {
                if (_editingOrder != null)
                {
                    SelectedLpu = Lpus.FirstOrDefault(l => l.LpuId == _editingOrder.LpuId) ?? Lpus[0];
                }
            }

            if (_editingOrder != null)
            {
                SelectedDoctor = FilteredDoctors.FirstOrDefault(d => d.DocId == _editingOrder.DocId);
            }

            if (Materials.Count > 0) SelectedMaterial = Materials[0];
            if (Departments.Count > 0) SelectedDepartment = Departments[0];

            if (_editingOrder != null)
            {
                await ApplyEditingOrderAsync();
                MarkExistingAnalysisSelections();
            }
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

    private async Task ApplyEditingOrderAsync()
    {
        if (_editingOrder == null)
            return;

        Department = _editingOrder.OrderLpuDepartment ?? string.Empty;
        OrderStatus = _editingOrder.OrderStatus;
        OrderIsCountingInContract = _editingOrder.OrderIsCountingInContract;

        if (_editingOrder.BarcodeMaterials != null)
        {
            foreach (var bm in _editingOrder.BarcodeMaterials)
            {
                var materialId = bm.MaterialId ?? 0;
                MaterialRows.Add(new OrderMaterialRow
                {
                    MaterialType = bm.Material?.MaterialName ?? string.Empty,
                    Ids = bm.BarcodeMatId.ToString("0"),
                    TakenDate = string.Empty,
                    Comment = string.Empty,
                    BarcodeMatId = bm.BarcodeMatId,
                    MaterialId = materialId,
                    AnalysisDepId = bm.AnalysisDepId
                });

                if (bm.BarcodeAnalysises != null)
                {
                    foreach (var ba in bm.BarcodeAnalysises)
                    {
                        SummaryRows.Add(new OrderSummaryRow
                        {
                            Ids = bm.BarcodeMatId.ToString("0"),
                            Code = ba.Analysis?.AnalysisCodeName ?? string.Empty,
                            Cipher = Cipher,
                            Name = ba.Analysis?.AnalysisName ?? string.Empty,
                            AnalysisId = ba.AnalysisId,
                            BarcodeMatId = bm.BarcodeMatId
                        });
                    }
                }
            }
        }
    }

    private void MarkExistingAnalysisSelections()
    {
        if (_editingOrder?.BarcodeMaterials == null)
            return;

        var selectedAnalyses = new Dictionary<(long AnalysisId, int AnalysisDepId), decimal>();
        foreach (var bm in _editingOrder.BarcodeMaterials)
        {
            if (bm.BarcodeAnalysises == null)
                continue;

            foreach (var ba in bm.BarcodeAnalysises)
            {
                selectedAnalyses[(ba.AnalysisId, ba.AnalysisDepId)] = bm.BarcodeMatId;
            }
        }

        foreach (var item in AnalysisChoices)
        {
            if (selectedAnalyses.TryGetValue((item.AnalysisId, item.AnalysisDepId), out var barcodeId))
            {
                item.IsSelected = true;
                item.BarcodeMatId = barcodeId;
            }
        }
    }

    private async Task LoadAnalysesForSelectedLpuAsync()
    {
        AnalysisChoices.Clear();
        DepartmentCategories.Clear();
        MaterialRows.Clear();
        SummaryRows.Clear();
        _selectedLpuContractAnalyses = [];

        if (SelectedLpu == null)
            return;

        try
        {
            var lpuContracts = await AppServices.Api.GetLpuContractsAsync(SelectedLpu.LpuId);
            if (lpuContracts.Count == 0)
                return;

            var activeContracts = lpuContracts.Where(c => c.ConLpuIsActive).ToList();
            if (activeContracts.Count == 0)
                activeContracts = lpuContracts;

            var contractAnalyses = new List<ContractAnalysisDto>();
            foreach (var lpuContract in activeContracts)
            {
                if (lpuContract.Contract == null)
                    continue;

                var analyses = await AppServices.Api.GetContractAnalysesByContractAsync(lpuContract.Contract.ContractId);
                contractAnalyses.AddRange(analyses);
            }

            _selectedLpuContractAnalyses = contractAnalyses
                .Where(a => a.Analysis != null)
                .GroupBy(a => a.AnalysisId)
                .Select(g => g.First())
                .ToList();

            foreach (var contractAnalysis in _selectedLpuContractAnalyses)
            {
                AnalysisChoices.Add(new AnalysisSelectionItem
                {
                    Code = contractAnalysis.Analysis?.AnalysisCodeName ?? string.Empty,
                    Name = contractAnalysis.Analysis?.AnalysisName ?? string.Empty,
                    AnalysisId = contractAnalysis.AnalysisId,
                    AnalysisDepId = contractAnalysis.Analysis?.AnalysisDepId ?? 0,
                    ToggleCommand = ToggleAnalysisCommand
                });
            }
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
        if (SelectedDepartment == null)
            return;

        var depAnalyses = _selectedLpuContractAnalyses
            .Where(a => a.Analysis?.AnalysisDepId == SelectedDepartment.AnalysisDepId)
            .ToList();

        foreach (var a in depAnalyses.Take(8))
            DepartmentCategories.Add(new AnalysisSelectionItem
            {
                Code = a.Analysis?.AnalysisCodeName ?? string.Empty,
                Name = a.Analysis?.AnalysisName ?? string.Empty,
                AnalysisId = a.AnalysisId,
                AnalysisDepId = SelectedDepartment.AnalysisDepId,
                ToggleCommand = ToggleAnalysisCommand
            });

        foreach (var a in depAnalyses)
            AnalysisChoices.Add(new AnalysisSelectionItem
            {
                Code = a.Analysis?.AnalysisCodeName ?? string.Empty,
                Name = a.Analysis?.AnalysisName ?? string.Empty,
                AnalysisId = a.AnalysisId,
                AnalysisDepId = SelectedDepartment.AnalysisDepId,
                ToggleCommand = ToggleAnalysisCommand
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

        var existingRow = MaterialRows.FirstOrDefault(m => m.MaterialId == material.MaterialId && m.AnalysisDepId == item.AnalysisDepId);
        if (existingRow != null)
        {
            item.BarcodeMatId = existingRow.BarcodeMatId;
            item.IsSelected = true;
            SummaryRows.Add(new OrderSummaryRow
            {
                Ids = existingRow.Ids,
                Code = item.Code,
                Cipher = Cipher,
                Name = item.Name,
                AnalysisId = item.AnalysisId,
                BarcodeMatId = existingRow.BarcodeMatId
            });
            return;
        }

        item.IsSelected = true;

        MaterialRows.Add(new OrderMaterialRow
        {
            MaterialType = material.MaterialName,
            TakenDate = TakenDate.ToString("dd.MM.yyyy"),
            Comment = string.Empty,
            MaterialId = material.MaterialId,
            AnalysisDepId = item.AnalysisDepId
        });

        SummaryRows.Add(new OrderSummaryRow
        {
            Code = item.Code,
            Cipher = Cipher,
            Name = item.Name,
            AnalysisId = item.AnalysisId,
        });
    }

    private void RemoveAnalysisItem(AnalysisSelectionItem item)
    {
        if (item == null || !item.IsSelected)
            return;

        var barcode = item.BarcodeMatId;
        item.IsSelected = false;
        item.BarcodeMatId = null;

        if (!barcode.HasValue)
            return;

        var summaryRow = SummaryRows.FirstOrDefault(s => s.BarcodeMatId == barcode.Value && s.AnalysisId == item.AnalysisId);
        if (summaryRow != null)
            SummaryRows.Remove(summaryRow);

        var remainingSummaryForBarcode = SummaryRows.Any(s => s.BarcodeMatId == barcode.Value);
        if (!remainingSummaryForBarcode)
        {
            var materialRow = MaterialRows.FirstOrDefault(m => m.BarcodeMatId == barcode.Value);
            if (materialRow != null)
                MaterialRows.Remove(materialRow);
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
                OrderStatus = OrderStatus,
                OrderLpuDepartment = Department,
                OrderIsCountingInContract = OrderIsCountingInContract
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

    private async Task UpdateOrderAsync()
    {
        try
        {
            if (_editingOrder == null)
                return;

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
                OrderStatus = OrderStatus,
                OrderLpuDepartment = Department,
                OrderIsCountingInContract = OrderIsCountingInContract
            };

            await AppServices.Api.UpdateOrderAsync(_editingOrder.OrderId, order);
            StatusMessage = "Заказ обновлён";
            _shell.Navigate(NavSection.Registration);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private Task SubmitOrderAsync() => _editingOrder != null ? UpdateOrderAsync() : CreateOrderAsync();
}
