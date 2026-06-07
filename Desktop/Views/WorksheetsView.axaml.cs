using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Threading;
using Desktop.Models;
using Desktop.ViewModels;

namespace Desktop.Views;

public partial class WorksheetsView : UserControl
{
    private WorksheetsViewModel? _vm;
    private bool _rebuildScheduled;

    public WorksheetsView()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (_vm != null)
        {
            _vm.AllAnalyses.CollectionChanged -= OnDataChanged;
            _vm.ExpandedRows.CollectionChanged -= OnDataChanged;
            _vm = null;
        }

        if (DataContext is WorksheetsViewModel vm)
        {
            _vm = vm;
            vm.AllAnalyses.CollectionChanged += OnDataChanged;
            vm.ExpandedRows.CollectionChanged += OnDataChanged;

            ScheduleRebuild();
        }
    }

    private void OnDataChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ScheduleRebuild();
    }

    private void ScheduleRebuild()
    {
        if (_rebuildScheduled)
            return;
        _rebuildScheduled = true;

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            _rebuildScheduled = false;
            if (_vm != null)
                RebuildAll(_vm);
        }, DispatcherPriority.Background);
    }

    private void RebuildAll(WorksheetsViewModel vm)
    {
        WorksheetGrid.ItemsSource = null;
        WorksheetGrid.Columns.Clear();

        WorksheetGrid.Columns.Add(new DataGridTextColumn
        {
            Header = "Штрих код",
            Binding = new Binding("BiomaterialBarcode"),
            Width = DataGridLength.SizeToCells,
            MinWidth = 100
        });

        WorksheetGrid.Columns.Add(new DataGridTextColumn
        {
            Header = "Вид",
            Binding = new Binding("Kind"),
            Width = DataGridLength.SizeToCells,
            MinWidth = 100
        });

        foreach (var analysisName in vm.AllAnalyses)
        {
            var colName = analysisName;
            WorksheetGrid.Columns.Add(new DataGridTemplateColumn
            {
                Header = colName,
                Width = DataGridLength.SizeToCells,
                MinWidth = 50,
                CellTemplate = new FuncDataTemplate<WorksheetExpandedRow>((row, _) =>
                {
                    var hasAnalysis = row.Analyses.TryGetValue(colName, out var has) && has;
                    return new TextBlock
                    {
                        Text = hasAnalysis ? "●" : "",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 14
                    };
                })
            });
        }

        WorksheetGrid.ItemsSource = vm.ExpandedRows;
    }
}