using System;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using Desktop.Models;

namespace Desktop.ViewModels;

public class CreatePatientViewModel : ViewModelBase
{
    private string _lastName = string.Empty;
    private string _firstName = string.Empty;
    private string _middleName = string.Empty;
    private string _birthDate = string.Empty;
    private string _gender = "Ж";
    private ReactiveCommand<Unit, Unit>? _okCommand;
    private ReactiveCommand<Unit, Unit>? _cancelCommand;

    public CreatePatientViewModel()
    {
        GenderOptions = new ObservableCollection<string> { "Ж", "М" };
    }

    public string LastName { get => _lastName; set => this.RaiseAndSetIfChanged(ref _lastName, value); }
    public string FirstName { get => _firstName; set => this.RaiseAndSetIfChanged(ref _firstName, value); }
    public string MiddleName { get => _middleName; set => this.RaiseAndSetIfChanged(ref _middleName, value); }
    public string BirthDate { get => _birthDate; set => this.RaiseAndSetIfChanged(ref _birthDate, value); }
    public string Gender { get => _gender; set => this.RaiseAndSetIfChanged(ref _gender, value); }

    public ObservableCollection<string> GenderOptions { get; }

    public ReactiveCommand<Unit, Unit>? OkCommand { get => _okCommand; set => this.RaiseAndSetIfChanged(ref _okCommand, value); }
    public ReactiveCommand<Unit, Unit>? CancelCommand { get => _cancelCommand; set => this.RaiseAndSetIfChanged(ref _cancelCommand, value); }
}
