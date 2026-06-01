using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Controls;
using Desktop.Models;
using Desktop.ViewModels;
using ReactiveUI;

namespace Desktop.Views;

public partial class CreateOrderView : ReactiveUserControl<CreateOrderViewModel>
{
    public CreateOrderView()
    {
        this.WhenActivated(_ => { });
        AvaloniaXamlLoader.Load(this);
    }

    private void OnAnalysisButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is AnalysisSelectionItem item)
        {
            ViewModel?.ToggleAnalysisCommand.Execute(item).Subscribe();
        }
    }
}
