using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Desktop.ViewModels;
using System;
using Desktop.Views;
using Desktop.Services;
using Desktop.Models;
using Avalonia.Interactivity;

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

    private async void PatientsGrid_DoubleTapped(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is RegistrationViewModel vm && vm.SelectedPatient != null)
        {
            var p = vm.SelectedPatient;
            var createVm = new CreatePatientViewModel
            {
                FirstName = p.PatientFirstName ?? string.Empty,
                LastName = p.PatientSecondName ?? string.Empty,
                MiddleName = p.PatientLastName ?? string.Empty,
                BirthDate = p.PatientBirthday?.ToDateTime(TimeOnly.MinValue),
                Gender = p.PatientGender ?? "Ж"
            };

            if (vm.ShowCreatePatientDialog != null)
            {
                var updated = await vm.ShowCreatePatientDialog(createVm);
                if (updated != null)
                {
                    try
                    {
                        await AppServices.Api.UpdatePatientAsync(p.PatientId, updated);
                        vm.SearchCommand.Execute().Subscribe();
                        vm.StatusMessage = "Пациент обновлён";
                    }
                    catch (Exception ex)
                    {
                        vm.StatusMessage = ex.Message;
                    }
                }
            }
        }
    }

    private async void OrdersGrid_DoubleTapped(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is RegistrationViewModel vm)
        {
            if (sender is DataGrid dg && dg.SelectedItem is OrderDto order)
            {
                try
                {
                    await vm.EditOrderAsync(order);
                }
                catch (Exception ex)
                {
                    vm.StatusMessage = ex.Message;
                }
            }
        }
    }
}
