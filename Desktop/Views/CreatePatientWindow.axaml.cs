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
            if (vm.FirstName == null || vm.LastName == null || vm.BirthDate == null)
            {
                ErrorsBlock.Text = "Не заполнены необходимые данные";
            }
            else
            {

                var p = new PatientDto();
                p.PatientLastName = vm.MiddleName;
                p.PatientFirstName = vm.FirstName;
                p.PatientSecondName = vm.LastName;
                if (DateTime.TryParse(vm.BirthDate, out var bd))
                    p.PatientBirthday = DateOnly.FromDateTime(bd);
                p.PatientGender = vm.Gender;
                Close(p);
            }
        });

        vm.CancelCommand = ReactiveUI.ReactiveCommand.Create(() => Close(null));

        return this.ShowDialog<PatientDto?>(owner);
    }
}
