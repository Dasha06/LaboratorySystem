using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase? _currentContent;

    public MainWindowViewModel()
    {
        AppServices.Initialize();
        CurrentContent = new LoginViewModel(this);
    }

    public ViewModelBase? CurrentContent
    {
        get => _currentContent;
        set => this.RaiseAndSetIfChanged(ref _currentContent, value);
    }

    public void OnLoggedIn() => CurrentContent = new ShellViewModel(this);

    public void OnLoggedOut()
    {
        AppServices.Session.ClearAuth();
        CurrentContent = new LoginViewModel(this);
    }
}
