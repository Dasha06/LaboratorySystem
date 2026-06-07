using System.Reactive;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private string _host = AppServices.BaseUrl;
    private string _login = string.Empty;
    private string _password = string.Empty;
    private string? _errorMessage;
    private bool _isBusy;
    private bool _isHostVisible;

    public LoginViewModel(MainWindowViewModel main)
    {
        _main = main;
        LoginCommand = ReactiveCommand.CreateFromTask(LoginAsync);
        ToggleHostCommand = ReactiveCommand.Create(() => { IsHostVisible = !IsHostVisible; });
    }

    public bool IsHostVisible
    {
        get => _isHostVisible;
        set => this.RaiseAndSetIfChanged(ref _isHostVisible, value);
    }

    public string Host
    {
        get => _host;
        set => this.RaiseAndSetIfChanged(ref _host, value);
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
    public ReactiveCommand<Unit, Unit> ToggleHostCommand { get; }

    private async Task LoginAsync()
    {
        try
        {
            IsBusy = true;
            ErrorMessage = null;
            if (!string.IsNullOrWhiteSpace(Host) && Host != AppServices.BaseUrl)
                AppServices.ReinitializeApi(Host);
            var worker = await AppServices.Api.LoginAsync(Login, Password);
            AppServices.Session.CurrentWorker = worker;
            _main.OnLoggedIn();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message.Contains("404") || ex.Message.Contains("NotFound")
                ? "Неверный логин или пароль"
                : $"Ошибка входа: {ex.Message}";
            
        }
        finally
        {
            IsBusy = false;
            
        }
    }
}
