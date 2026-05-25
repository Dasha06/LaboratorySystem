using System.Collections.ObjectModel;
using System.Reactive;
using Desktop.Models;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class ResultsViewModel : ViewModelBase
{
    private int _filterTabIndex;
    private string? _statusMessage;
    private readonly List<ResultRow> _allRows = [];

    public ResultsViewModel(ShellViewModel shell)
    {
        Rows = new ObservableCollection<ResultRow>();
        SetFilterCommand = ReactiveCommand.Create<object?>(p =>
        {
            FilterTabIndex = p switch
            {
                int i => i,
                string s when int.TryParse(s, out var n) => n,
                _ => 0
            };
        });
        RefreshCommand = ReactiveCommand.CreateFromTask(LoadAsync);
        _ = LoadAsync();
    }

    public ObservableCollection<ResultRow> Rows { get; }
    public int FilterTabIndex
    {
        get => _filterTabIndex;
        set
        {
            this.RaiseAndSetIfChanged(ref _filterTabIndex, value);
            ApplyFilter();
        }
    }

    public string? StatusMessage { get => _statusMessage; set => this.RaiseAndSetIfChanged(ref _statusMessage, value); }
    public ReactiveCommand<object?, Unit> SetFilterCommand { get; }
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    private async Task LoadAsync()
    {
        try
        {
            var analyses = await AppServices.Api.GetBarcodeAnalysesAsync();
            var orders = await AppServices.Api.GetOrdersAsync();
            _allRows.Clear();

            foreach (var ba in analyses)
            {
                var bm = ba.BarcodeMaterial;
                var order = bm?.OrderId != null
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

                _allRows.Add(new ResultRow
                {
                    Ids = ba.BarcodeId.ToString("0"),
                    FullName = patient?.FullName ?? "Баженова Дарья Александровна",
                    Lpu = "текст",
                    Doctor = "Ещё какой-то текст",
                    MaterialType = bm?.Material?.MaterialName ?? "И тут тоже текст",
                    HasResult = !string.IsNullOrEmpty(ba.Result),
                    IsSent = false
                });
            }

            ApplyFilter();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private void ApplyFilter()
    {
        Rows.Clear();
        IEnumerable<ResultRow> filtered = _allRows;
        filtered = FilterTabIndex switch
        {
            0 => filtered.Where(r => r.HasResult),
            1 => filtered.Where(r => !r.HasResult),
            2 => filtered.Where(r => r.IsSent),
            _ => filtered
        };
        foreach (var r in filtered)
            Rows.Add(r);
    }
}
