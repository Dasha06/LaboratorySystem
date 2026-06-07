using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Desktop.Models;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class DepartmentListItem : ReactiveObject
{
    private int _tripodCount;

    public AnalysisDepartmentDto Department { get; set; } = null!;
    public int TripodCount
    {
        get => _tripodCount;
        set => this.RaiseAndSetIfChanged(ref _tripodCount, value);
    }

    public string DisplayText => $"{Department.AnalysisDepName} ({TripodCount})";
    public int AnalysisDepId => Department.AnalysisDepId;
}

public class RacksViewModel : ViewModelBase
{
    private readonly ShellViewModel _shell;
    private DepartmentListItem? _selectedDepartmentItem;
    private TripodDto? _selectedTripod;
    private int _rows = 10;
    private int _columns = 10;
    private string _newTripodName = string.Empty;
    private string? _statusMessage;
    private string? _scannedBarcode;
    private RackCellState? _selectedCell;
    private string _scannedPatientInfo = string.Empty;

    public RacksViewModel(ShellViewModel shell)
    {
        _shell = shell;
        Departments = new ObservableCollection<DepartmentListItem>();
        Tripods = new ObservableCollection<TripodDto>();
        RackCells = new ObservableCollection<RackCellState>();
        Priorities = new ObservableCollection<PriorityItem>();
        GridSizeOptions = new ObservableCollection<string> { "10x10", "5x10", "2x10", "2x5" };
        LoadCommand = ReactiveCommand.CreateFromTask(LoadAsync);
        OpenWorksheetsCommand = ReactiveCommand.Create(_shell.OpenWorksheets,
            this.WhenAnyValue(x => x.SelectedTripod).Select((TripodDto? t) => t != null));
        CreateTripodCommand = ReactiveCommand.CreateFromTask(CreateTripodAsync,
            this.WhenAnyValue(x => x.NewTripodName, x => x.SelectedDepartmentItem,
                (name, dep) => !string.IsNullOrWhiteSpace(name) && dep != null));
        ScanBarcodeCommand = ReactiveCommand.CreateFromTask(ScanBarcodeAsync,
            this.WhenAnyValue(x => x.ScannedBarcode, x => x.SelectedCell,
                (string? bc, RackCellState? cell) => !string.IsNullOrWhiteSpace(bc) && cell != null));
        _ = LoadAsync();
    }

    public ObservableCollection<DepartmentListItem> Departments { get; }
    public ObservableCollection<TripodDto> Tripods { get; }
    public ObservableCollection<RackCellState> RackCells { get; }
    public ObservableCollection<PriorityItem> Priorities { get; }

    public DepartmentListItem? SelectedDepartmentItem
    {
        get => _selectedDepartmentItem;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDepartmentItem, value);
            _ = FilterTripodsAsync();
        }
    }

    public TripodDto? SelectedTripod
    {
        get => _selectedTripod;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTripod, value);
            if (value != null)
            {
                AppServices.Session.SelectedTripodId = value.TripodId;
                _ = LoadRackAsync(value.TripodId);
            }
        }
    }

    public bool HasTripods => Tripods.Count > 0;

    public int Rows
    {
        get => _rows;
        set => this.RaiseAndSetIfChanged(ref _rows, value);
    }

    public int Columns
    {
        get => _columns;
        set => this.RaiseAndSetIfChanged(ref _columns, value);
    }

    public ObservableCollection<string> GridSizeOptions { get; }

    private string? _selectedGridSizeOption;
    public string? SelectedGridSizeOption
    {
        get => _selectedGridSizeOption;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedGridSizeOption, value);
            if (value != null)
            {
                var parts = value.Split('x');
                if (parts.Length == 2 && int.TryParse(parts[0], out var r) && int.TryParse(parts[1], out var c))
                {
                    Rows = r;
                    Columns = c;
                    _ = ApplySelectedGridSizeAsync();
                }
            }
        }
    }

    public string NewTripodName
    {
        get => _newTripodName;
        set => this.RaiseAndSetIfChanged(ref _newTripodName, value);
    }

    public string? StatusMessage { get => _statusMessage; set => this.RaiseAndSetIfChanged(ref _statusMessage, value); }

    public string? ScannedBarcode
    {
        get => _scannedBarcode;
        set => this.RaiseAndSetIfChanged(ref _scannedBarcode, value);
    }

    public RackCellState? SelectedCell
    {
        get => _selectedCell;
        set => this.RaiseAndSetIfChanged(ref _selectedCell, value);
    }

    public string ScannedPatientInfo
    {
        get => _scannedPatientInfo;
        set => this.RaiseAndSetIfChanged(ref _scannedPatientInfo, value);
    }

    public ReactiveCommand<Unit, Unit> LoadCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenWorksheetsCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateTripodCommand { get; }
    public ReactiveCommand<Unit, Unit> ScanBarcodeCommand { get; }

    private bool _isApplyingGridSize = false;

    private async Task ApplySelectedGridSizeAsync()
    {
        if (_isApplyingGridSize)
            return;
        if (SelectedTripod == null || SelectedGridSizeOption == null)
            return;

        try
        {
            _isApplyingGridSize = true;
            var maxCell = Rows * Columns;
            await AppServices.Api.UpdateTripodAsync(SelectedTripod.TripodId, SelectedTripod.TripodName, maxCell, SelectedTripod.AnalysisDepartmentId);
            SelectedTripod.TripodMaxCell = maxCell;
            await LoadRackAsync(SelectedTripod.TripodId);
            StatusMessage = "Размер штатива обновлён";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            _isApplyingGridSize = false;
        }
    }

    private List<TripodDto> _allTripods = [];

    private async Task LoadAsync()
    {
        try
        {
            var prevDepId = SelectedDepartmentItem?.AnalysisDepId;
            var deps = await AppServices.Api.GetAnalysisDepartmentsAsync();
            _allTripods = await AppServices.Api.GetTripodsAsync();

            Departments.Clear();
            foreach (var d in deps)
            {
                var count = _allTripods.Count(t => t.AnalysisDepartmentId == d.AnalysisDepId);
                Departments.Add(new DepartmentListItem
                {
                    Department = d,
                    TripodCount = count
                });
            }

            if (Departments.Count > 0)
            {
                // Restore previously selected department or default to first
                var toSelect = prevDepId.HasValue
                    ? Departments.FirstOrDefault(d => d.AnalysisDepId == prevDepId.Value)
                    : null;
                SelectedDepartmentItem = toSelect ?? Departments[0];
            }
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task FilterTripodsAsync()
    {
        Tripods.Clear();
        if (SelectedDepartmentItem == null)
            return;
        var depId = SelectedDepartmentItem.AnalysisDepId;
        foreach (var t in _allTripods.Where(x => x.AnalysisDepartmentId == depId))
            Tripods.Add(t);
        this.RaisePropertyChanged(nameof(HasTripods));
        SelectedTripod = Tripods.FirstOrDefault();
        await Task.CompletedTask;
    }

    private async Task LoadRackAsync(long tripodId)
    {
        try
        {
            var items = await AppServices.Api.GetTripodBarcodeMaterialsByTripodAsync(tripodId);
            var tripod = SelectedTripod;
            var maxCell = tripod?.TripodMaxCell ?? 100;

            if (maxCell == 100)
            {
                Rows = 10; Columns = 10; SelectedGridSizeOption = "10x10";
            }
            else if (maxCell == 50)
            {
                Rows = 5; Columns = 10; SelectedGridSizeOption = "5x10";
            }
            else if (maxCell == 20)
            {
                Rows = 2; Columns = 10; SelectedGridSizeOption = "2x10";
            }
            else if (maxCell == 10)
            {
                Rows = 2; Columns = 5; SelectedGridSizeOption = "2x5";
            }

            SelectedCell = null;
            ScannedBarcode = null;
            ScannedPatientInfo = string.Empty;

            RackCells.Clear();
            for (var i = 0; i < maxCell; i++)
            {
                var cell = new RackCellState
                {
                    Index = i,
                    IsOccupied = i < items.Count,
                    SelectCommand = ReactiveCommand.Create<RackCellState>(SelectCell)
                };

                if (cell.IsOccupied)
                {
                    var tbm = items[i];
                    var material = tbm.BarcodeMaterial?.Material;
                    var typeName = material?.MaterialName ?? string.Empty;
                    cell.MaterialType = typeName;
                    cell.BarcodeMatId = tbm.BarcodeMatId;
                    cell.AnalysisDepId = tbm.AnalysisDepId;
                    cell.Barcode = tbm.BarcodeMatId.ToString("0");

                    try
                    {
                        var mat = tbm.BarcodeMaterial?.BarcodeMatId;
                        var s = mat?.ToString() ?? string.Empty;
                        s = new string(s.Where(char.IsDigit).ToArray());
                        cell.Label = s.Length >= 4 ? s.Substring(s.Length - 4) : s;
                    }
                    catch
                    {
                        cell.Label = string.Empty;
                    }

                    cell.PatientName = string.Empty;
                }
                else
                {
                    cell.MaterialType = string.Empty;
                    cell.Label = string.Empty;
                    cell.Barcode = string.Empty;
                    cell.PatientName = string.Empty;
                }

                RackCells.Add(cell);
            }

            Priorities.Clear();
            var analysisGroups = items
                .SelectMany(t => t.BarcodeMaterial?.BarcodeAnalysises ?? [])
                .Where(ba => ba.Analysis != null)
                .GroupBy(ba => ba.Analysis!.AnalysisName)
                .Select(g => new PriorityItem { Text = $"{g.Key} — {g.Count()} анализов" })
                .Take(5);
            var idx = 1;
            foreach (var p in analysisGroups)
            {
                Priorities.Add(new PriorityItem { Text = $"{idx}. {p.Text}" });
                idx++;
            }
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateTripodAsync()
    {
        if (SelectedDepartmentItem == null)
        {
            StatusMessage = "Выберите отделение из списка слева";
            return;
        }

        var name = NewTripodName.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            StatusMessage = "Введите название штатива";
            return;
        }

        try
        {
            var maxCell = Rows * Columns;
            var currentDepId = SelectedDepartmentItem.AnalysisDepId;
            await AppServices.Api.CreateTripodAsync(name, maxCell, currentDepId);
            StatusMessage = "Штатив создан";
            NewTripodName = string.Empty;
            await LoadAsync();
            SelectedTripod = Tripods.FirstOrDefault(t => t.TripodName == name) ?? SelectedTripod;
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    public void SelectCell(RackCellState cell)
    {
        if (SelectedCell != null && SelectedCell != cell)
        {
            SelectedCell.IsSelected = false;
        }

        cell.IsSelected = !cell.IsSelected;

        if (cell.IsSelected)
        {
            SelectedCell = cell;
            ScannedBarcode = null;
            ScannedPatientInfo = string.Empty;
            StatusMessage = $"Выбрана ячейка {cell.Index + 1}. Отсканируйте штрих-код пробирки.";

            if (cell.IsOccupied)
            {
                ScannedPatientInfo = !string.IsNullOrEmpty(cell.PatientName)
                    ? $"{cell.Barcode} — {cell.PatientName}"
                    : cell.Barcode;
                StatusMessage = $"Ячейка {cell.Index + 1}: {ScannedPatientInfo}";
            }
        }
        else
        {
            SelectedCell = null;
            ScannedBarcode = null;
            ScannedPatientInfo = string.Empty;
        }
    }

    private RackCellState? FindFirstEmptyCell()
    {
        foreach (var cell in RackCells.OrderBy(c => c.Index))
        {
            if (!cell.IsOccupied)
                return cell;
        }
        return null;
    }

    private async Task ScanBarcodeAsync()
    {
        if (SelectedCell == null)
        {
            var emptyCell = FindFirstEmptyCell();
            if (emptyCell == null)
            {
                StatusMessage = "Ошибка: все ячейки штатива заняты.";
                ScannedBarcode = null;
                return;
            }

            emptyCell.IsSelected = true;
            SelectedCell = emptyCell;
            StatusMessage = $"Автоматически выбрана ячейка {emptyCell.Index + 1}.";
        }

        var barcodeText = ScannedBarcode?.Trim();
        if (string.IsNullOrWhiteSpace(barcodeText))
        {
            StatusMessage = "Штрих-код не может быть пустым.";
            return;
        }

        if (!decimal.TryParse(barcodeText, out var barcodeMatId))
        {
            StatusMessage = "Некорректный формат штрих-кода.";
            return;
        }

        try
        {
            StatusMessage = "Поиск пробирки в базе данных...";

            var barcodeMaterial = await AppServices.Api.GetBarcodeMaterialByBarcodeAsync(barcodeMatId);

            if (barcodeMaterial == null)
            {
                StatusMessage = "Ошибка: данного биоматериала нет в базе.";
                return;
            }

            if (SelectedTripod != null && barcodeMaterial.AnalysisDepId != SelectedTripod.AnalysisDepartmentId)
            {
                StatusMessage = "Ошибка: отделение пробирки не соответствует отделению штатива.";
                return;
            }

            var alreadyInTripod = RackCells.Any(c =>
                c.IsOccupied && c.BarcodeMatId == barcodeMatId && c != SelectedCell);
            if (alreadyInTripod)
            {
                StatusMessage = "Ошибка: данная пробирка уже привязана к другому слоту этого штатива.";
                return;
            }

            var patientName = string.Empty;
            if (barcodeMaterial.OrderId.HasValue)
            {
                try
                {
                    var orderDetails = await AppServices.Api.GetOrderDetailsAsync(barcodeMaterial.OrderId.Value);
                    patientName = orderDetails.Patient?.FullName ?? string.Empty;
                }
                catch
                {
                    // ignore
                }
            }

            var materialName = barcodeMaterial.Material?.MaterialName ?? string.Empty;
            ScannedPatientInfo = $"{barcodeMatId:0} — {patientName} ({materialName})".TrimEnd(' ', '(', ')');

            if (SelectedCell!.IsOccupied && SelectedCell.BarcodeMatId != barcodeMatId)
            {
                try
                {
                    await AppServices.Api.DeleteTripodBarcodeMaterialAsync(
                        SelectedTripod!.TripodId, SelectedCell.BarcodeMatId);
                }
                catch
                {
                    // ignore
                }
            }

            await AppServices.Api.CreateTripodBarcodeMaterialAsync(
                SelectedTripod!.TripodId, barcodeMatId, barcodeMaterial.AnalysisDepId);

            SelectedCell.IsOccupied = true;
            SelectedCell.BarcodeMatId = barcodeMatId;
            SelectedCell.AnalysisDepId = barcodeMaterial.AnalysisDepId;
            SelectedCell.Barcode = barcodeMatId.ToString("0");
            SelectedCell.PatientName = patientName;
            SelectedCell.MaterialType = materialName;

            var s = barcodeMatId.ToString("0");
            s = new string(s.Where(char.IsDigit).ToArray());
            SelectedCell.Label = s.Length >= 4 ? s.Substring(s.Length - 4) : s;

            await LoadRackAsync(SelectedTripod.TripodId);

            StatusMessage = $"Пробирка {barcodeMatId:0} привязана к ячейке {SelectedCell.Index + 1}. " +
                            (string.IsNullOrEmpty(patientName) ? "" : $"Пациент: {patientName}");

            ScannedBarcode = null;
            SelectedCell = null;
            ScannedPatientInfo = string.Empty;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }
}