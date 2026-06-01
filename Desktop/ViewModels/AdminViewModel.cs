using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Desktop.Models;
using Desktop.Services;
using ReactiveUI;

namespace Desktop.ViewModels;

public class AdminViewModel : ViewModelBase
{
    private readonly ShellViewModel _shell;
    private string _newRoleName = string.Empty;
    private string _newWorkerName = string.Empty;
    private string _newWorkerLogin = string.Empty;
    private string _newWorkerPassword = string.Empty;
    private string _newMaterialName = string.Empty;
    private string _newDepartmentName = string.Empty;
    private string _newAnalysisName = string.Empty;
    private string _newAnalysisCode = string.Empty;
    private string _newMeasurementName = string.Empty;
    private string _newLpuName = string.Empty;
    private string _newLpuEmail = string.Empty;
    private string _newContractName = string.Empty;
    private int _newContractMoney;
    private double _newContractRemainsMoney;
    private bool _newLpuContractIsActive = true;
    private double _newContractAnalysisCost;
    private string? _statusMessage;
    private RoleDto? _selectedRoleForWorker;
    private ContractDto? _selectedContractForLpu;
    private ContractDto? _selectedContractForAnalysis;
    private AnalysiseDto? _selectedAnalysisForContract;
    private AnalysisDepartmentDto? _selectedAnalysisDepartmentForAnalysis;

    public AdminViewModel(ShellViewModel shell)
    {
        _shell = shell;
        Roles = new ObservableCollection<RoleDto>();
        Workers = new ObservableCollection<WorkerDto>();
        Materials = new ObservableCollection<MaterialDto>();
        Departments = new ObservableCollection<AnalysisDepartmentDto>();
        Analyses = new ObservableCollection<AnalysiseDto>();
        Measurements = new ObservableCollection<MeasurementDto>();
        Lpus = new ObservableCollection<LpuDto>();
        Contracts = new ObservableCollection<ContractDto>();
        ContractAnalysisRows = new ObservableCollection<ContractAnalysisDto>();

        LoadDataCommand = ReactiveCommand.CreateFromTask(LoadDataAsync);
        CreateRoleCommand = ReactiveCommand.CreateFromTask(CreateRoleAsync, this.WhenAnyValue(x => x.NewRoleName).Select(name => !string.IsNullOrWhiteSpace(name)));
        CreateWorkerCommand = ReactiveCommand.CreateFromTask(CreateWorkerAsync, this.WhenAnyValue(x => x.NewWorkerName, x => x.NewWorkerLogin, x => x.NewWorkerPassword,
            (name, login, password) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password)));
        CreateMaterialCommand = ReactiveCommand.CreateFromTask(CreateMaterialAsync, this.WhenAnyValue(x => x.NewMaterialName).Select(name => !string.IsNullOrWhiteSpace(name)));
        CreateDepartmentCommand = ReactiveCommand.CreateFromTask(CreateDepartmentAsync, this.WhenAnyValue(x => x.NewDepartmentName).Select(name => !string.IsNullOrWhiteSpace(name)));
        CreateAnalysisCommand = ReactiveCommand.CreateFromTask(CreateAnalysisAsync, this.WhenAnyValue(x => x.NewAnalysisName).Select(name => !string.IsNullOrWhiteSpace(name)));
        CreateMeasurementCommand = ReactiveCommand.CreateFromTask(CreateMeasurementAsync, this.WhenAnyValue(x => x.NewMeasurementName).Select(name => !string.IsNullOrWhiteSpace(name)));
        CreateLpuCommand = ReactiveCommand.CreateFromTask(CreateLpuAsync, this.WhenAnyValue(x => x.NewLpuName).Select(name => !string.IsNullOrWhiteSpace(name)));
        CreateContractCommand = ReactiveCommand.CreateFromTask(CreateContractAsync, this.WhenAnyValue(x => x.NewContractName).Select(name => !string.IsNullOrWhiteSpace(name)));
        CreateLpuContractCommand = ReactiveCommand.CreateFromTask(CreateLpuContractAsync, this.WhenAnyValue(x => x.SelectedContractForLpu, x => x.SelectedLpuForContract, (contract, lpu) => contract != null && lpu != null));
        CreateContractAnalysisCommand = ReactiveCommand.CreateFromTask(CreateContractAnalysisAsync, this.WhenAnyValue(x => x.SelectedContractForAnalysis, x => x.SelectedAnalysisForContract, (contract, analysis) => contract != null && analysis != null));

        this.WhenAnyValue(x => x.SelectedContractForAnalysis)
            .Subscribe(async _ => await LoadContractAnalysesAsync());

        LoadDataCommand.Execute().Subscribe();
    }

    public ObservableCollection<RoleDto> Roles { get; }
    public ObservableCollection<WorkerDto> Workers { get; }
    public ObservableCollection<MaterialDto> Materials { get; }
    public ObservableCollection<AnalysisDepartmentDto> Departments { get; }
    public ObservableCollection<AnalysiseDto> Analyses { get; }
    public ObservableCollection<MeasurementDto> Measurements { get; }
    public ObservableCollection<LpuDto> Lpus { get; }
    public ObservableCollection<ContractDto> Contracts { get; }
    public ObservableCollection<ContractAnalysisDto> ContractAnalysisRows { get; }

    public string NewRoleName { get => _newRoleName; set => this.RaiseAndSetIfChanged(ref _newRoleName, value); }
    public string NewWorkerName { get => _newWorkerName; set => this.RaiseAndSetIfChanged(ref _newWorkerName, value); }
    public string NewWorkerLogin { get => _newWorkerLogin; set => this.RaiseAndSetIfChanged(ref _newWorkerLogin, value); }
    public string NewWorkerPassword { get => _newWorkerPassword; set => this.RaiseAndSetIfChanged(ref _newWorkerPassword, value); }
    public string NewMaterialName { get => _newMaterialName; set => this.RaiseAndSetIfChanged(ref _newMaterialName, value); }
    public string NewDepartmentName { get => _newDepartmentName; set => this.RaiseAndSetIfChanged(ref _newDepartmentName, value); }
    public string NewAnalysisName { get => _newAnalysisName; set => this.RaiseAndSetIfChanged(ref _newAnalysisName, value); }
    public string NewAnalysisCode { get => _newAnalysisCode; set => this.RaiseAndSetIfChanged(ref _newAnalysisCode, value); }
    public string NewMeasurementName { get => _newMeasurementName; set => this.RaiseAndSetIfChanged(ref _newMeasurementName, value); }
    public string NewLpuName { get => _newLpuName; set => this.RaiseAndSetIfChanged(ref _newLpuName, value); }
    public string NewLpuEmail { get => _newLpuEmail; set => this.RaiseAndSetIfChanged(ref _newLpuEmail, value); }
    public string NewContractName { get => _newContractName; set => this.RaiseAndSetIfChanged(ref _newContractName, value); }
    public int NewContractMoney { get => _newContractMoney; set => this.RaiseAndSetIfChanged(ref _newContractMoney, value); }
    public double NewContractRemainsMoney { get => _newContractRemainsMoney; set => this.RaiseAndSetIfChanged(ref _newContractRemainsMoney, value); }
    public bool NewLpuContractIsActive { get => _newLpuContractIsActive; set => this.RaiseAndSetIfChanged(ref _newLpuContractIsActive, value); }
    public double NewContractAnalysisCost { get => _newContractAnalysisCost; set => this.RaiseAndSetIfChanged(ref _newContractAnalysisCost, value); }
    public string? StatusMessage { get => _statusMessage; set => this.RaiseAndSetIfChanged(ref _statusMessage, value); }

    public RoleDto? SelectedRoleForWorker { get => _selectedRoleForWorker; set => this.RaiseAndSetIfChanged(ref _selectedRoleForWorker, value); }
    public AnalysisDepartmentDto? SelectedAnalysisDepartmentForAnalysis { get => _selectedAnalysisDepartmentForAnalysis; set => this.RaiseAndSetIfChanged(ref _selectedAnalysisDepartmentForAnalysis, value); }
    public ContractDto? SelectedContractForLpu { get => _selectedContractForLpu; set => this.RaiseAndSetIfChanged(ref _selectedContractForLpu, value); }
    public ContractDto? SelectedContractForAnalysis { get => _selectedContractForAnalysis; set => this.RaiseAndSetIfChanged(ref _selectedContractForAnalysis, value); }
    public AnalysiseDto? SelectedAnalysisForContract { get => _selectedAnalysisForContract; set => this.RaiseAndSetIfChanged(ref _selectedAnalysisForContract, value); }
    public LpuDto? SelectedLpuForContract { get; set; }

    public ReactiveCommand<Unit, Unit> LoadDataCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateRoleCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateWorkerCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateMaterialCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateDepartmentCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateAnalysisCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateMeasurementCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateLpuCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateContractCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateLpuContractCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateContractAnalysisCommand { get; }

    private async Task LoadDataAsync()
    {
        try
        {
            Roles.Clear();
            Workers.Clear();
            Materials.Clear();
            Departments.Clear();
            Analyses.Clear();
            Measurements.Clear();
            Lpus.Clear();
            Contracts.Clear();
            ContractAnalysisRows.Clear();

            foreach (var role in await AppServices.Api.GetRolesAsync())
                Roles.Add(role);
            foreach (var worker in await AppServices.Api.GetWorkersAsync())
                Workers.Add(worker);
            foreach (var material in await AppServices.Api.GetMaterialsAsync())
                Materials.Add(material);
            foreach (var department in await AppServices.Api.GetAnalysisDepartmentsAsync())
                Departments.Add(department);
            foreach (var analysis in await AppServices.Api.GetAnalysesAsync())
                Analyses.Add(analysis);
            foreach (var measurement in await AppServices.Api.GetMeasurementsAsync())
                Measurements.Add(measurement);
            foreach (var lpu in await AppServices.Api.GetLpusAsync())
                Lpus.Add(lpu);
            foreach (var contract in await AppServices.Api.GetContractsAsync())
                Contracts.Add(contract);

            StatusMessage = "Данные загружены";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task LoadContractAnalysesAsync()
    {
        if (SelectedContractForAnalysis == null)
            return;

        try
        {
            ContractAnalysisRows.Clear();
            foreach (var row in await AppServices.Api.GetContractAnalysesByContractAsync(SelectedContractForAnalysis.ContractId))
                ContractAnalysisRows.Add(row);
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateRoleAsync()
    {
        try
        {
            await AppServices.Api.CreateRoleAsync(NewRoleName);
            NewRoleName = string.Empty;
            await LoadDataAsync();
            StatusMessage = "Роль создана";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateWorkerAsync()
    {
        try
        {
            var worker = new WorkerDto
            {
                WorkerFio = NewWorkerName,
                WorkerLogin = NewWorkerLogin,
                WorkerPassword = NewWorkerPassword
            };

            await AppServices.Api.CreateWorkerAsync(worker);
            if (SelectedRoleForWorker != null)
            {
                var createdWorkers = await AppServices.Api.GetWorkersAsync();
                var created = createdWorkers.FirstOrDefault(w => w.WorkerLogin == NewWorkerLogin);
                if (created != null)
                    await AppServices.Api.UpdateWorkerRolesAsync(created.WorkerId, new[] { SelectedRoleForWorker.RoleId });
            }

            NewWorkerName = string.Empty;
            NewWorkerLogin = string.Empty;
            NewWorkerPassword = string.Empty;
            SelectedRoleForWorker = null;
            await LoadDataAsync();
            StatusMessage = "Работник создан";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateMaterialAsync()
    {
        try
        {
            await AppServices.Api.CreateMaterialAsync(NewMaterialName);
            NewMaterialName = string.Empty;
            await LoadDataAsync();
            StatusMessage = "Материал создан";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateDepartmentAsync()
    {
        try
        {
            await AppServices.Api.CreateAnalysisDepartmentAsync(NewDepartmentName);
            NewDepartmentName = string.Empty;
            await LoadDataAsync();
            StatusMessage = "Отделение анализов создано";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateAnalysisAsync()
    {
        try
        {
            await AppServices.Api.CreateAnalysisAsync(NewAnalysisName, SelectedAnalysisDepartmentForAnalysis?.AnalysisDepId, NewAnalysisCode);
            NewAnalysisName = string.Empty;
            NewAnalysisCode = string.Empty;
            SelectedAnalysisDepartmentForAnalysis = null;
            await LoadDataAsync();
            StatusMessage = "Анализ создан";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateMeasurementAsync()
    {
        try
        {
            await AppServices.Api.CreateMeasurementAsync(NewMeasurementName);
            NewMeasurementName = string.Empty;
            await LoadDataAsync();
            StatusMessage = "Единица измерения создана";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateLpuAsync()
    {
        try
        {
            await AppServices.Api.CreateLpuAsync(NewLpuName, string.IsNullOrWhiteSpace(NewLpuEmail) ? null : NewLpuEmail);
            NewLpuName = string.Empty;
            NewLpuEmail = string.Empty;
            await LoadDataAsync();
            StatusMessage = "ЛПУ создано";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateContractAsync()
    {
        try
        {
            await AppServices.Api.CreateContractAsync(NewContractName, NewContractMoney, NewContractRemainsMoney);
            NewContractName = string.Empty;
            NewContractMoney = 0;
            NewContractRemainsMoney = 0;
            await LoadDataAsync();
            StatusMessage = "Контракт создан";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateLpuContractAsync()
    {
        if (SelectedContractForLpu == null || SelectedLpuForContract == null)
            return;

        try
        {
            await AppServices.Api.CreateLpuContractAsync(SelectedContractForLpu.ContractId, SelectedLpuForContract.LpuId, NewLpuContractIsActive);
            NewLpuContractIsActive = true;
            await LoadDataAsync();
            StatusMessage = "Связь ЛПУ и контракта создана";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task CreateContractAnalysisAsync()
    {
        if (SelectedContractForAnalysis == null || SelectedAnalysisForContract == null)
            return;

        try
        {
            await AppServices.Api.CreateContractAnalysisAsync(SelectedContractForAnalysis.ContractId, SelectedAnalysisForContract.AnalysisId, NewContractAnalysisCost);
            NewContractAnalysisCost = 0;
            await LoadContractAnalysesAsync();
            StatusMessage = "Связь анализа и контракта создана";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }
}
