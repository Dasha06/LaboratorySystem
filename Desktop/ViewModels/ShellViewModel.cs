using System.Reactive;
using Desktop.Models;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class ShellViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _main;
    private ViewModelBase _currentPage;
    private NavSection _activeSection = NavSection.Registration;

    public ShellViewModel(MainWindowViewModel main)
    {
        _main = main;
        _currentPage = new RegistrationViewModel(this);
        NavigateCommand = ReactiveCommand.Create<string>(s => Navigate(Enum.Parse<NavSection>(s)));
        LogoutCommand = ReactiveCommand.Create(() => _main.OnLoggedOut());
        OpenWorksheetsCommand = ReactiveCommand.Create(OpenWorksheets);
    }

    public string UserDisplayName => AppServices.Session.CurrentWorker?.WorkerFio ?? "Пользователь";

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    public NavSection ActiveSection
    {
        get => _activeSection;
        set => this.RaiseAndSetIfChanged(ref _activeSection, value);
    }

    public bool IsRegistrationActive => ActiveSection == NavSection.Registration;
    public bool IsTrackerActive => ActiveSection == NavSection.Tracker;
    public bool IsRacksActive => ActiveSection == NavSection.Racks || ActiveSection == NavSection.Worksheets;
    public bool IsResultsActive => ActiveSection == NavSection.Results;
    public bool IsReportsActive => ActiveSection == NavSection.Reports;
    public bool IsWorkflowsActive => ActiveSection == NavSection.Workflows;

    public ReactiveCommand<string, Unit> NavigateCommand { get; }
    public ReactiveCommand<Unit, Unit> LogoutCommand { get; }
    public ReactiveCommand<Unit, Unit> OpenWorksheetsCommand { get; }

    public void Navigate(NavSection section)
    {
        ActiveSection = section;
        AppServices.Session.ActiveNavSection = section;
        this.RaisePropertyChanged(nameof(IsRegistrationActive));
        this.RaisePropertyChanged(nameof(IsTrackerActive));
        this.RaisePropertyChanged(nameof(IsRacksActive));
        this.RaisePropertyChanged(nameof(IsResultsActive));
        this.RaisePropertyChanged(nameof(IsReportsActive));
        this.RaisePropertyChanged(nameof(IsWorkflowsActive));
        CurrentPage = section switch
        {
            NavSection.Registration => new RegistrationViewModel(this),
            NavSection.Tracker => new TrackerViewModel(this),
            NavSection.Racks => new RacksViewModel(this),
            NavSection.Results => new ResultsViewModel(this),
            NavSection.Reports => new ReportsViewModel(this),
            NavSection.Workflows => new WorkflowsViewModel(this),
            NavSection.Worksheets => new WorksheetsViewModel(this),
            _ => new RegistrationViewModel(this)
        };
    }

    public void OpenWorksheets()
    {
        if (!AppServices.Session.SelectedTripodId.HasValue)
            return;
        AppServices.Session.ReturnNavAfterWorksheets = NavSection.Racks;
        ActiveSection = NavSection.Worksheets;
        this.RaisePropertyChanged(nameof(IsRacksActive));
        CurrentPage = new WorksheetsViewModel(this);
    }

    public void OpenCreateOrder(PatientDto patient)
    {
        AppServices.Session.SelectedPatient = patient;
        CurrentPage = new CreateOrderViewModel(this, patient);
    }

    public void BackFromCreateOrder()
    {
        Navigate(NavSection.Registration);
    }

    public void BackFromWorksheets()
    {
        Navigate(NavSection.Racks);
    }
}
