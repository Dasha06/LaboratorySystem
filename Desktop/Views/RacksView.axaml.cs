using Avalonia.Controls;
using Avalonia.Input;
using Desktop.Models;
using Desktop.ViewModels;

namespace Desktop.Views;

public partial class RacksView : UserControl
{
    public RacksView()
    {
        InitializeComponent();
    }

    private void OnCellPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is RackCellState cell)
        {
            if (DataContext is RacksViewModel vm)
            {
                vm.SelectCell(cell);
                e.Handled = true;
            }
        }
    }
}