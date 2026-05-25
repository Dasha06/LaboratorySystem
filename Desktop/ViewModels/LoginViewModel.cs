using System.Reactive;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private string _login = string.Empty;
    private string _password = string.Empty;
    private string? _errorMessage;
    private bool _isBusy;

    public LoginViewModel(MainWindowViewModel main)
    {
        _main = main;
        LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
    }

    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => this.RaiseAndSetIfChanged(ref _isBusy, value);
    }

    public ReactiveCommand<Unit, Unit> LoginCommand { get; }

    private async Task LoginAsync()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;
            var worker = await AppServices.Api.LoginAsync(Login, Password);
            AppServices.Session.CurrentWorker = worker;
            _main.OnLoggedIn();
            Console.WriteLine($"Logged in as {worker.WorkerFio}");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message.Contains("404") || ex.Message.Contains("NotFound")
                ? "Неверный логин или пароль"
                : $"Ошибка входа: {ex.Message}";
            
            Console.WriteLine($"Error: {ErrorMessage}");
        }
        finally
        {
            IsBusy = false;
            
            Console.WriteLine($"IsBusy: {IsBusy}");
        }
    }
}
