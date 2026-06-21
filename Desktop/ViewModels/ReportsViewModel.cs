using System.Collections.ObjectModel;
using System.Reactive;
using Desktop.Models;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class ReportsViewModel : ViewModelBase
{
    private readonly LaboratoryApiClient _api;
    private ReportTypeItem? _selectedReportType;
    private DateTime? _dateFrom;
    private DateTime? _dateTo;
    private LpuDto? _selectedLpu;
    private bool _isLpuSelectionVisible;
    private bool _isLoading;
    private bool _hasNoData;

    public ReportsViewModel(ShellViewModel shell)
    {
        _api = AppServices.Api;

        ReportTypes = new ObservableCollection<ReportTypeItem>
        {
            new() { Name = "1. Заказы по работникам за период", ReportId = 1 },
            new() { Name = "2. Заказанные анализы за период", ReportId = 2 },
            new() { Name = "3. Разрешенные услуги", ReportId = 4 },
        };
        _selectedReportType = ReportTypes[0];
        _selectedReportType.IsSelected = true;

        Lpus = new ObservableCollection<LpuDto>();
        WorkerRows = new ObservableCollection<WorkerReportRow>();
        OrderedAnalysisRows = new ObservableCollection<OrderedAnalysisReportRow>();
        AvailableAnalysisRows = new ObservableCollection<AvailableAnalysisReportRow>();

        BuildReportCommand = ReactiveCommand.CreateFromTask(BuildReportAsync);


        // Load LPUs for selection
        _ = LoadLpusAsync();
    }

    public ObservableCollection<ReportTypeItem> ReportTypes { get; }
    public ObservableCollection<LpuDto> Lpus { get; }
    public ObservableCollection<WorkerReportRow> WorkerRows { get; }
    public ObservableCollection<OrderedAnalysisReportRow> OrderedAnalysisRows { get; }
    public ObservableCollection<AvailableAnalysisReportRow> AvailableAnalysisRows { get; }

    public ReactiveCommand<Unit, Unit> BuildReportCommand { get; }

    public ReportTypeItem? SelectedReportType
    {
        get => _selectedReportType;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedReportType, value);
            if (value != null)
            {
                foreach (var r in ReportTypes)
                    r.IsSelected = r == value;
                IsLpuSelectionVisible = value.ReportId == 3 || value.ReportId == 4;
                
                // Clear previous results
                WorkerRows.Clear();
                OrderedAnalysisRows.Clear();
                AvailableAnalysisRows.Clear();
                HasNoData = false;
                
                // Notify XAML that visibility bindings changed
                this.RaisePropertyChanged(nameof(IsReport1));
                this.RaisePropertyChanged(nameof(IsReport2));
                this.RaisePropertyChanged(nameof(IsReport3));
                this.RaisePropertyChanged(nameof(IsReport4));
            }
        }
    }

    public DateTime? DateFrom
    {
        get => _dateFrom;
        set => this.RaiseAndSetIfChanged(ref _dateFrom, value);
    }

    public DateTime? DateTo
    {
        get => _dateTo;
        set => this.RaiseAndSetIfChanged(ref _dateTo, value);
    }

    public LpuDto? SelectedLpu
    {
        get => _selectedLpu;
        set => this.RaiseAndSetIfChanged(ref _selectedLpu, value);
    }

    public bool IsLpuSelectionVisible
    {
        get => _isLpuSelectionVisible;
        set => this.RaiseAndSetIfChanged(ref _isLpuSelectionVisible, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    public bool HasNoData
    {
        get => _hasNoData;
        set => this.RaiseAndSetIfChanged(ref _hasNoData, value);
    }

    // Which report type is currently selected
    public bool IsReport1 => SelectedReportType?.ReportId == 1;
    public bool IsReport2 => SelectedReportType?.ReportId == 2;
    public bool IsReport3 => SelectedReportType?.ReportId == 3;
    public bool IsReport4 => SelectedReportType?.ReportId == 4;

    private async Task LoadLpusAsync()
    {
        try
        {
            var lpus = await _api.GetAllLpusAsync();
            Lpus.Clear();
            foreach (var l in lpus)
                Lpus.Add(l);
        }
        catch
        {
            // Ignore loading errors
        }
    }

    private async Task BuildReportAsync()
    {
        if (SelectedReportType == null) return;

        IsLoading = true;
        HasNoData = false;
        WorkerRows.Clear();
        OrderedAnalysisRows.Clear();
        AvailableAnalysisRows.Clear();

        try
        {
            switch (SelectedReportType.ReportId)
            {
                case 1:
                    await BuildReport1Async();
                    break;
                case 2:
                    await BuildReport2Async();
                    break;
                case 3:
                    await BuildReport3Async();
                    break;
                case 4:
                    await BuildReport4Async();
                    break;
            }
        }
        catch
        {
            HasNoData = true;
        }
        finally
        {
            IsLoading = false;
            this.RaisePropertyChanged(nameof(IsReport1));
            this.RaisePropertyChanged(nameof(IsReport2));
            this.RaisePropertyChanged(nameof(IsReport4));
        }
    }

    private async Task BuildReport1Async()
    {
        var from = DateFrom ?? DateTime.Now.AddMonths(-1);
        var to = DateTo ?? DateTime.Now;

        var data = await _api.GetOrdersCountByWorkerAsync(from, to);
        if (data.Count == 0)
        {
            HasNoData = true;
            return;
        }

        foreach (var kv in data)
            WorkerRows.Add(new WorkerReportRow { WorkerFio = kv.Key, OrdersCount = kv.Value });
    }

    private async Task BuildReport2Async()
    {
        var from = DateFrom ?? DateTime.Now.AddMonths(-1);
        var to = DateTo ?? DateTime.Now;

        var data = await _api.GetOrderedAnalysesBetweenAsync(from, to);
        if (data.Count == 0)
        {
            HasNoData = true;
            return;
        }

        foreach (var row in data)
            OrderedAnalysisRows.Add(row);
    }

    private async Task BuildReport3Async()
    {
        if (SelectedLpu == null)
        {
            HasNoData = true;
            return;
        }

        var from = DateFrom ?? DateTime.Now.AddMonths(-1);
        var to = DateTo ?? DateTime.Now;

        var data = await _api.GetOrderedAnalysesByLpuAsync(from, to, SelectedLpu.LpuId);
        if (data.Count == 0)
        {
            HasNoData = true;
            return;
        }

        foreach (var row in data)
            OrderedAnalysisRows.Add(row);
    }

    private async Task BuildReport4Async()
    {
        if (SelectedLpu == null)
        {
            HasNoData = true;
            return;
        }

        var data = await _api.GetAvailableAnalysesByLpuAsync(SelectedLpu.LpuId);
        if (data.Count == 0)
        {
            HasNoData = true;
            return;
        }

        foreach (var row in data)
            AvailableAnalysisRows.Add(row);
    }
}