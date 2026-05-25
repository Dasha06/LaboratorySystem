using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
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
}
