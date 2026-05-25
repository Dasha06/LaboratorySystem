using System.Collections.ObjectModel;
using System.Reactive;
using Desktop.Models;
using ReactiveUI;

namespace Desktop.ViewModels;

public class ReportsViewModel : ViewModelBase
{
    private ReportFileItem? _selectedReportFile;

    public ReportsViewModel(ShellViewModel shell)
    {
        ReportFiles = new ObservableCollection<ReportFileItem>();
        ServiceRows = new ObservableCollection<ReportServiceRow>();
        for (var i = 1; i <= 8; i++)
            ReportFiles.Add(new ReportFileItem { Name = $"FILENAME{i:D6}", IsSelected = i == 3 });
        _selectedReportFile = ReportFiles.FirstOrDefault(f => f.IsSelected);
        SelectFileCommand = ReactiveCommand.Create<ReportFileItem>(SelectFile);

        ServiceRows.Add(new ReportServiceRow { Cipher = "Выборгская МБ", ServiceCode = "10-011", Service = "ТЕКСТ", Quantity = 0, Price = "1000 руб." });
        ServiceRows.Add(new ReportServiceRow { Cipher = "Выборгская МБ", ServiceCode = "10-012", Service = "ТЕКСТ", Quantity = 0, Price = "1000 руб." });
        ServiceRows.Add(new ReportServiceRow { Cipher = "Выборгская МБ", ServiceCode = "10-013", Service = "ТЕКСТ", Quantity = 0, Price = "1000 руб." });
    }

    public ObservableCollection<ReportFileItem> ReportFiles { get; }
    public ObservableCollection<ReportServiceRow> ServiceRows { get; }

    public ReportFileItem? SelectedReportFile
    {
        get => _selectedReportFile;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedReportFile, value);
            if (value != null)
                SelectFile(value);
        }
    }

    public ReactiveCommand<ReportFileItem, Unit> SelectFileCommand { get; }

    private void SelectFile(ReportFileItem item)
    {
        foreach (var f in ReportFiles)
            f.IsSelected = f == item;
    }
}
