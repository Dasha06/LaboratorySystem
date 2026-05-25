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

    public ReactiveCommand<Unit, Unit> BackCommand { get; }
    public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

    private async Task LoadAsync()
    {
        try
        {
            var tripodId = AppServices.Session.SelectedTripodId;
            if (!tripodId.HasValue)
            {
                StatusMessage = "Штатив не выбран";
                return;
            }

            var rows = await AppServices.Api.GetWorksheetsAsync(tripodId.Value);
            
            // Extract all unique analyses
            var allAnalysesSet = new HashSet<string>();
            foreach (var row in rows)
            {
                if (!string.IsNullOrEmpty(row.Analyses))
                {
                    var analyses = row.Analyses.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var a in analyses)
                        allAnalysesSet.Add(a.Trim());
                }
            }

            AllAnalyses.Clear();
            foreach (var a in allAnalysesSet.OrderBy(x => x))
                AllAnalyses.Add(a);

            // Transform rows
            ExpandedRows.Clear();
            foreach (var row in rows)
            {
                var expandedRow = new WorksheetExpandedRow
                {
                    BiomaterialBarcode = row.BiomaterialBarcode,
                    Kind = row.Kind
                };

                // Parse analyses
                if (!string.IsNullOrEmpty(row.Analyses))
                {
                    var rowAnalyses = row.Analyses.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToHashSet();
                    
                    foreach (var analysis in AllAnalyses)
                        expandedRow.Analyses[analysis] = rowAnalyses.Contains(analysis);
                }

                ExpandedRows.Add(expandedRow);
            }

            StatusMessage = rows.Count == 0 ? "Нет материалов на штативе" : null;
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }
}
