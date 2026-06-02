using Avalonia.Controls;
using Desktop.Models;
using Desktop.ViewModels;
using System;
using System.Threading.Tasks;

namespace Desktop.Views;

public partial class CreatePatientWindow : Window
{
    public CreatePatientWindow()
    {
        InitializeComponent();
    }

    public Task<PatientDto?> ShowForViewModelAsync(CreatePatientViewModel vm, Window owner)
    {
        DataContext = vm;

        vm.OkCommand = ReactiveUI.ReactiveCommand.Create(() =>
        {
            ErrorsBlock.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(vm.FirstName) || string.IsNullOrWhiteSpace(vm.MiddleName) || !vm.BirthDate.HasValue)
            {
                ErrorsBlock.Text = "Не заполнены необходимые данные";
                return;
            }

            var p = new PatientDto();
            p.PatientSecondName = vm.MiddleName;
            p.PatientFirstName = vm.FirstName;
            p.PatientLastName = vm.LastName;
            p.PatientBirthday = DateOnly.FromDateTime(vm.BirthDate.Value);
            p.PatientGender = vm.Gender;
            Close(p);
        });

        vm.CancelCommand = ReactiveUI.ReactiveCommand.Create(() => Close(null));

        return this.ShowDialog<PatientDto?>(owner);
    }
}
