using System.Collections.ObjectModel;
using System.Reactive;
using Desktop.Models;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class WorksheetsViewModel : ViewModelBase
{
    private readonly ShellViewModel _shell;
    private string? _statusMessage;
    private string _tripodName = string.Empty;
    private bool _isLoaded;

    public WorksheetsViewModel(ShellViewModel shell)
    {
        _shell = shell;
        ExpandedRows = new ObservableCollection<WorksheetExpandedRow>();
        AllAnalyses = new ObservableCollection<string>();
        BackCommand = ReactiveCommand.Create(_shell.BackFromWorksheets);
        RefreshCommand = ReactiveCommand.CreateFromTask(LoadAsync);
        _ = LoadAsync();
    }

    public ObservableCollection<WorksheetExpandedRow> ExpandedRows { get; }
    public ObservableCollection<string> AllAnalyses { get; }
    public string? StatusMessage { get => _statusMessage; set => this.RaiseAndSetIfChanged(ref _statusMessage, value); }
    public string TripodName
    {
        get => _tripodName;
        set => this.RaiseAndSetIfChanged(ref _tripodName, value);
    }

    public bool IsLoaded
    {
        get => _isLoaded;
        private set => this.RaiseAndSetIfChanged(ref _isLoaded, value);
    }

    public ReactiveCommand<Unit, Unit> BackCommand { get; }
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    private async Task LoadAsync()
    {
        IsLoaded = false;
        try
        {
            var tripodId = AppServices.Session.SelectedTripodId;
            if (!tripodId.HasValue)
            {
                StatusMessage = "Штатив не выбран";
                return;
            }

            var tripods = await AppServices.Api.GetTripodsAsync();
            var tripod = tripods.FirstOrDefault(t => t.TripodId == tripodId.Value);
            TripodName = tripod?.TripodName ?? $"Штатив #{tripodId}";

            var rows = await AppServices.Api.GetWorksheetsAsync(tripodId.Value);

            // Extract all unique analysis names
            var allAnalysesList = new List<string>();
            var allAnalysesSet = new HashSet<string>();
            foreach (var row in rows)
            {
                if (!string.IsNullOrEmpty(row.Analyses))
                {
                    var analyses = row.Analyses.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var a in analyses)
                    {
                        var trimmed = a.Trim();
                        if (allAnalysesSet.Add(trimmed))
                            allAnalysesList.Add(trimmed);
                    }
                }
            }
            allAnalysesList.Sort();

            // Prepare rows
            var preparedRows = new List<WorksheetExpandedRow>();
            foreach (var row in rows)
            {
                var expandedRow = new WorksheetExpandedRow
                {
                    BiomaterialBarcode = row.BiomaterialBarcode,
                    Kind = row.Kind
                };

                if (!string.IsNullOrEmpty(row.Analyses))
                {
                    var rowAnalyses = row.Analyses
                        .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToHashSet();

                    foreach (var a in allAnalysesList)
                        expandedRow.Analyses[a] = rowAnalyses.Contains(a);
                }
                else
                {
                    foreach (var a in allAnalysesList)
                        expandedRow.Analyses[a] = false;
                }

                preparedRows.Add(expandedRow);
            }

            // Populate collections atomically
            AllAnalyses.Clear();
            foreach (var a in allAnalysesList)
                AllAnalyses.Add(a);

            ExpandedRows.Clear();
            foreach (var r in preparedRows)
                ExpandedRows.Add(r);

            StatusMessage = rows.Count == 0 ? "Нет материалов на штативе" : null;
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
        finally
        {
            IsLoaded = true;
        }
    }
}