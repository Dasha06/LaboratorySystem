using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Desktop.Models;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class RacksViewModel : ViewModelBase
{
    private readonly ShellViewModel _shell;
    private AnalysisDepartmentDto? _selectedDepartment;
    private TripodDto? _selectedTripod;
    private int _gridSize = 10;
    private int _rows = 10;
    private int _columns = 10;
    private string _newTripodName = string.Empty;
    private string? _statusMessage;

    public RacksViewModel(ShellViewModel shell)
    {
        _shell = shell;
        Departments = new ObservableCollection<AnalysisDepartmentDto>();
        Tripods = new ObservableCollection<TripodDto>();
        RackCells = new ObservableCollection<RackCellState>();
        Priorities = new ObservableCollection<PriorityItem>();
        GridSizeOptions = new ObservableCollection<string> { "10x10", "5x10", "2x10", "2x5" };
        LoadCommand = ReactiveCommand.CreateFromTask(LoadAsync);
        OpenWorksheetsCommand = ReactiveCommand.Create(_shell.OpenWorksheets,
            this.WhenAnyValue(x => x.SelectedTripod).Select((TripodDto? t) => t != null));
        CreateTripodCommand = ReactiveCommand.CreateFromTask(CreateTripodAsync,
            this.WhenAnyValue(x => x.NewTripodName, x => x.SelectedDepartment,
                (name, dep) => !string.IsNullOrWhiteSpace(name) && dep != null));
        _ = LoadAsync();
    }

    public ObservableCollection<AnalysisDepartmentDto> Departments { get; }
    public ObservableCollection<TripodDto> Tripods { get; }
    public ObservableCollection<RackCellState> RackCells { get; }
    public ObservableCollection<PriorityItem> Priorities { get; }

    public AnalysisDepartmentDto? SelectedDepartment
    {
        get => _selectedDepartment;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDepartment, value);
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

    public int GridSize
    {
        get => _gridSize;
        set => this.RaiseAndSetIfChanged(ref _gridSize, value);
    }

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
                }
            }
        }
    }

    public string RackSizeLabel => $"Размер штатива: {Rows}x{Columns}";
    public string NewTripodName
    {
        get => _newTripodName;
        set => this.RaiseAndSetIfChanged(ref _newTripodName, value);
    }

    public string? StatusMessage { get => _statusMessage; set => this.RaiseAndSetIfChanged(ref _statusMessage, value); }

    public ReactiveCommand<Unit, Unit> LoadCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenWorksheetsCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateTripodCommand { get; }

    private List<TripodDto> _allTripods = [];

    private async Task LoadAsync()
    {
        try
        {
            var deps = await AppServices.Api.GetAnalysisDepartmentsAsync();
            Departments.Clear();
            foreach (var d in deps)
                Departments.Add(d);
            if (Departments.Count > 0)
                SelectedDepartment = Departments[0];

            _allTripods = await AppServices.Api.GetTripodsAsync();
            await FilterTripodsAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task FilterTripodsAsync()
    {
        Tripods.Clear();
        if (SelectedDepartment == null)
            return;
        foreach (var t in _allTripods.Where(x => x.AnalysisDepartmentId == SelectedDepartment.AnalysisDepId))
            Tripods.Add(t);
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

            // Choose sensible default rows/columns for common sizes, allow user to change via options
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

            RackCells.Clear();
            for (var i = 0; i < maxCell; i++)
            {
                var cell = new RackCellState { Index = i, IsOccupied = i < items.Count };
                if (cell.IsOccupied)
                {
                    var material = items[i].BarcodeMaterial?.Material;
                    var typeName = material?.MaterialName ?? string.Empty;
                    cell.MaterialType = typeName;

                    try
                    {
                        var mat = items[i].BarcodeMaterial?.BarcodeMatId;
                        var s = mat?.ToString() ?? string.Empty;
                        s = new string(s.Where(char.IsDigit).ToArray());
                        if (s.Length >= 4)
                            cell.Label = s.Substring(s.Length - 4);
                        else
                            cell.Label = s;
                    }
                    catch
                    {
                        cell.Label = string.Empty;
                    }
                }
                else
                {
                    cell.MaterialType = string.Empty;
                    cell.Label = string.Empty;
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
        if (SelectedDepartment == null)
        {
            StatusMessage = "Выберите отдел для нового штатива";
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
            await AppServices.Api.CreateTripodAsync(name, maxCell, SelectedDepartment.AnalysisDepId);
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
}
