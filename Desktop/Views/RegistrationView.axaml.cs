using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Desktop.ViewModels;
using System;
using Desktop.Views;

namespace Desktop.Views;

public partial class RegistrationView : UserControl
{
    public RegistrationView()
    {
        InitializeComponent();
        this.DataContextChanged += RegistrationView_DataContextChanged;
    }

    private void RegistrationView_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is RegistrationViewModel vm)
        {
            vm.ShowCreatePatientDialog = async (createVm) =>
            {
                var wnd = new CreatePatientWindow();
                var owner = TopLevel.GetTopLevel(this) as Window
                    ?? (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                if (owner == null)
                    throw new InvalidOperationException("Не удалось определить окно-владельца для диалога создания пациента.");

                return await wnd.ShowForViewModelAsync(createVm, owner);
            };
        }
    }
}
