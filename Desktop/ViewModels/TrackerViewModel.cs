using System.Collections.ObjectModel;
using System.Reactive;
using Desktop.Models;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class TrackerViewModel : ViewModelBase
{
    private string _idsFilter = string.Empty;
    private string _comment = string.Empty;
    private TrackerRow? _selectedRow;
    private int _selectedTabIndex;
    private string? _statusMessage;

    public TrackerViewModel(ShellViewModel shell)
    {
        SearchModes = ["По материалу", "По пациенту"];
        SelectedSearchMode = SearchModes[0];
        Results = new ObservableCollection<TrackerRow>();
        Measurements = new ObservableCollection<BarcodeAnalysiseDto>();
        SearchCommand = ReactiveCommand.CreateFromTask(SearchAsync);
        _ = SearchAsync();
    }

    public List<string> SearchModes { get; }
    public string SelectedSearchMode { get; set; } = "По материалу";
    public ObservableCollection<TrackerRow> Results { get; }
    public ObservableCollection<BarcodeAnalysiseDto> Measurements { get; }

    public string IdsFilter { get => _idsFilter; set => this.RaiseAndSetIfChanged(ref _idsFilter, value); }
    public string Comment { get => _comment; set => this.RaiseAndSetIfChanged(ref _comment, value); }

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
            var materials = await AppServices.Api.GetBarcodeMaterialsAsync();
            var allMaterials = await AppServices.Api.GetMaterialsAsync();
            var departments = await AppServices.Api.GetAnalysisDepartmentsAsync();
            var orders = await AppServices.Api.GetOrdersAsync();
            Results.Clear();

            foreach (var bm in materials)
            {
                var mat = bm.MaterialId.HasValue
                    ? allMaterials.FirstOrDefault(m => m.MaterialId == bm.MaterialId)
                    : null;
                var dep = departments.FirstOrDefault(d => d.AnalysisDepId == bm.AnalysisDepId);
                if (!string.IsNullOrWhiteSpace(IdsFilter) &&
                    !bm.BarcodeMatId.ToString("0").Contains(IdsFilter, StringComparison.OrdinalIgnoreCase))
                    continue;

                var order = bm.OrderId.HasValue
                    ? orders.FirstOrDefault(o => o.OrderId == bm.OrderId)
                    : null;
                PatientDto? patient = null;
                if (order != null)
                {
                    try
                    {
                        var details = await AppServices.Api.GetOrderDetailsAsync(order.OrderId);
                        patient = details.Patient;
                    }
                    catch { /* ignore */ }
                }

                Results.Add(new TrackerRow
                {
                    Comment = Comment,
                    Ids = bm.BarcodeMatId.ToString("0"),
                    FullName = patient?.FullName ?? "—",
                    Lpu = order != null ? "текст" : "—",
                    MaterialKind = dep?.AnalysisDepName ?? "—",
                    MaterialType = mat?.MaterialName ?? "—",
                    BarcodeMatId = bm.BarcodeMatId
                });
            }
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
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
