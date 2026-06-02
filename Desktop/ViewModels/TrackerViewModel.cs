using System.Collections.ObjectModel;
using System.Reactive;
using Desktop.Models;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class TrackerViewModel : ViewModelBase
{
    private string _idsFilter = string.Empty;
    private string _patientFirstNameFilter = string.Empty;
    private string _patientSecondNameFilter = string.Empty;
    private string _patientLastNameFilter = string.Empty;
    private DateTime? _patientBirthdayFilter;
    private string _comment = string.Empty;
    private TrackerRow? _selectedRow;
    private int _selectedTabIndex;
    private string? _statusMessage;
    private DateTime? _dateFrom;
    private DateTime? _dateTo;
    private string _selectedSearchMode = "По материалу";

    public TrackerViewModel(ShellViewModel shell)
    {
        // Инициализируем коллекции ДО установки SelectedSearchMode
        Results = new ObservableCollection<TrackerRow>();
        Measurements = new ObservableCollection<BarcodeAnalysiseDto>();
        SearchModes = ["По материалу", "По пациенту"];
        
        // Теперь можно безопасно присваивать SearchMode, т.к. Results уже инициализирована
        SelectedSearchMode = SearchModes[0];
        SearchCommand = ReactiveCommand.CreateFromTask(SearchAsync);
        _ = SearchAsync();
    }

    public List<string> SearchModes { get; }
    
    public string SelectedSearchMode 
    { 
        get => _selectedSearchMode; 
        set 
        { 
            this.RaiseAndSetIfChanged(ref _selectedSearchMode, value);
            // Очищаем фильтры при смене режима (проверяем null для безопасности)
            IdsFilter = string.Empty;
            PatientFirstNameFilter = string.Empty;
            PatientSecondNameFilter = string.Empty;
            PatientLastNameFilter = string.Empty;
            PatientBirthdayFilter = null;
            Results?.Clear();
        } 
    }
    
    public ObservableCollection<TrackerRow> Results { get; }
    public ObservableCollection<BarcodeAnalysiseDto> Measurements { get; }

    public string IdsFilter { get => _idsFilter; set => this.RaiseAndSetIfChanged(ref _idsFilter, value); }
    public string PatientFirstNameFilter { get => _patientFirstNameFilter; set => this.RaiseAndSetIfChanged(ref _patientFirstNameFilter, value); }
    public string PatientSecondNameFilter { get => _patientSecondNameFilter; set => this.RaiseAndSetIfChanged(ref _patientSecondNameFilter, value); }
    public string PatientLastNameFilter { get => _patientLastNameFilter; set => this.RaiseAndSetIfChanged(ref _patientLastNameFilter, value); }
    public DateTime? PatientBirthdayFilter { get => _patientBirthdayFilter; set => this.RaiseAndSetIfChanged(ref _patientBirthdayFilter, value); }
    public string Comment { get => _comment; set => this.RaiseAndSetIfChanged(ref _comment, value); }
    
    public DateTime? DateFrom { get => _dateFrom; set => this.RaiseAndSetIfChanged(ref _dateFrom, value); }
    public DateTime? DateTo { get => _dateTo; set => this.RaiseAndSetIfChanged(ref _dateTo, value); }

    public TrackerRow? SelectedRow
    {
        get => _selectedRow;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedRow, value);
            if (value != null && decimal.TryParse(value.Ids, out var id))
                _ = LoadMeasurementsAsync(id);
        }
    }

    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedTabIndex, value);
    }

    public string? StatusMessage { get => _statusMessage; set => this.RaiseAndSetIfChanged(ref _statusMessage, value); }
    public ReactiveCommand<Unit, Unit> SearchCommand { get; }

    private async Task SearchAsync()
    {
        try
        {
            Results.Clear();

            if (SelectedSearchMode == "По материалу")
                await SearchByMaterialAsync();
            else
                await SearchByPatientAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task SearchByMaterialAsync()
    {
        var materials = await AppServices.Api.GetBarcodeMaterialsAsync();
        var allMaterials = await AppServices.Api.GetMaterialsAsync();
        var departments = await AppServices.Api.GetAnalysisDepartmentsAsync();
        var orders = await AppServices.Api.GetOrdersAsync();

        foreach (var bm in materials)
        {
            var mat = bm.MaterialId.HasValue
                ? allMaterials.FirstOrDefault(m => m.MaterialId == bm.MaterialId)
                : null;
            var dep = departments.FirstOrDefault(d => d.AnalysisDepId == bm.AnalysisDepId);
            
            // Фильтр по штрих-коду
            if (!string.IsNullOrWhiteSpace(IdsFilter) &&
                !bm.BarcodeMatId.ToString("0").Contains(IdsFilter, StringComparison.OrdinalIgnoreCase))
                continue;

            var order = bm.OrderId.HasValue
                ? orders.FirstOrDefault(o => o.OrderId == bm.OrderId)
                : null;

            var orderDetails = order;
            if (orderDetails != null && (orderDetails.Lpu == null || orderDetails.Patient == null))
                orderDetails = await EnsureOrderDetailsAsync(orderDetails) ?? orderDetails;

            // Фильтр по датам создания заказа
            if (orderDetails != null && (DateFrom.HasValue || DateTo.HasValue))
            {
                var orderCreatedDate = orderDetails.CreatedAt ?? DateTime.Now;
                if (DateFrom.HasValue && orderCreatedDate.Date < DateFrom.Value.Date)
                    continue;
                if (DateTo.HasValue && orderCreatedDate.Date > DateTo.Value.Date)
                    continue;
            }
            else if (orderDetails == null && (DateFrom.HasValue || DateTo.HasValue))
            {
                // Пропускаем материалы без заказа, если задан фильтр по датам
                continue;
            }

            PatientDto? patient = orderDetails?.Patient;
            Results.Add(new TrackerRow
            {
                Comment = Comment,
                Ids = bm.BarcodeMatId.ToString("0"),
                FullName = patient?.FullName ?? "—",
                Lpu = orderDetails?.Lpu?.LpuName ?? "—",
                MaterialKind = dep?.AnalysisDepName ?? "—",
                MaterialType = mat?.MaterialName ?? "—",
                BarcodeMatId = bm.BarcodeMatId
            });
        }
    }

    private async Task<OrderDto?> EnsureOrderDetailsAsync(OrderDto order)
    {
        if (order.Lpu != null && order.Patient != null)
            return order;

        try
        {
            return await AppServices.Api.GetOrderDetailsAsync(order.OrderId);
        }
        catch
        {
            return order;
        }
    }

    private async Task SearchByPatientAsync()
    {
        var orders = await AppServices.Api.GetOrdersAsync();
        var allMaterials = await AppServices.Api.GetMaterialsAsync();
        var departments = await AppServices.Api.GetAnalysisDepartmentsAsync();
        var barcodeMaterials = await AppServices.Api.GetBarcodeMaterialsAsync();

        var materialsByOrderId = barcodeMaterials
            .Where(bm => bm.OrderId.HasValue)
            .GroupBy(bm => bm.OrderId!.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        var patientLastNameFilter = PatientLastNameFilter?.Trim();
        var patientFirstNameFilter = PatientFirstNameFilter?.Trim();
        var patientSecondNameFilter = PatientSecondNameFilter?.Trim();

        foreach (var order in orders)
        {
            var currentOrder = order;
            if (currentOrder.Lpu == null || currentOrder.Patient == null)
                currentOrder = await EnsureOrderDetailsAsync(currentOrder) ?? currentOrder;

            var patient = currentOrder.Patient;
            if (patient == null)
                continue;

            if (!string.IsNullOrWhiteSpace(patientLastNameFilter) &&
                (string.IsNullOrWhiteSpace(patient.PatientLastName) ||
                 !patient.PatientLastName.Contains(patientLastNameFilter, StringComparison.OrdinalIgnoreCase)))
                continue;

            if (!string.IsNullOrWhiteSpace(patientFirstNameFilter) &&
                (string.IsNullOrWhiteSpace(patient.PatientFirstName) ||
                 !patient.PatientFirstName.Contains(patientFirstNameFilter, StringComparison.OrdinalIgnoreCase)))
                continue;

            if (!string.IsNullOrWhiteSpace(patientSecondNameFilter) &&
                (string.IsNullOrWhiteSpace(patient.PatientSecondName) ||
                 !patient.PatientSecondName.Contains(patientSecondNameFilter, StringComparison.OrdinalIgnoreCase)))
                continue;

            if (PatientBirthdayFilter.HasValue)
            {
                if (!patient.PatientBirthday.HasValue ||
                    patient.PatientBirthday.Value != DateOnly.FromDateTime(PatientBirthdayFilter.Value))
                    continue;
            }

            if (!materialsByOrderId.TryGetValue(order.OrderId, out var orderMaterials) || orderMaterials.Count == 0)
                continue;

            foreach (var bm in orderMaterials)
            {
                var mat = bm.MaterialId.HasValue
                    ? allMaterials.FirstOrDefault(m => m.MaterialId == bm.MaterialId)
                    : null;
                var dep = departments.FirstOrDefault(d => d.AnalysisDepId == bm.AnalysisDepId);

                Results.Add(new TrackerRow
                {
                    Comment = Comment,
                    Ids = bm.BarcodeMatId.ToString("0"),
                    FullName = patient.FullName,
                    Lpu = currentOrder.Lpu?.LpuName ?? "—",
                    MaterialKind = dep?.AnalysisDepName ?? "—",
                    MaterialType = mat?.MaterialName ?? "—",
                    BarcodeMatId = bm.BarcodeMatId
                });
            }
        }
    }

    private async Task LoadMeasurementsAsync(decimal barcodeId)
    {
        try
        {
            var list = await AppServices.Api.GetBarcodeAnalysesByBarcodeAsync(barcodeId);
            Measurements.Clear();
            foreach (var m in list)
                Measurements.Add(m);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }
}
