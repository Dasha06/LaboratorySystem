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
    private string _editRoleName = string.Empty;
    private RoleDto? _selectedRoleForEdit;
    private RoleDto? _selectedRoleForDelete;

    private string _newWorkerName = string.Empty;
    private string _newWorkerLogin = string.Empty;
    private string _newWorkerPassword = string.Empty;
    private string _editWorkerName = string.Empty;
    private string _editWorkerLogin = string.Empty;
    private string _editWorkerPassword = string.Empty;
    private WorkerDto? _selectedWorkerForEdit;
    private WorkerDto? _selectedWorkerForDelete;
    private RoleDto? _selectedRoleForWorker;
    private RoleDto? _selectedRoleForEditWorker;

    private string _newMaterialName = string.Empty;
    private string _editMaterialName = string.Empty;
    private MaterialDto? _selectedMaterialForEdit;
    private MaterialDto? _selectedMaterialForDelete;

    private string _newDepartmentName = string.Empty;
    private string _editDepartmentName = string.Empty;
    private AnalysisDepartmentDto? _selectedDepartmentForEdit;
    private AnalysisDepartmentDto? _selectedDepartmentForDelete;

    private string _newAnalysisName = string.Empty;
    private string _newAnalysisCode = string.Empty;
    private string _editAnalysisName = string.Empty;
    private string _editAnalysisCode = string.Empty;
    private AnalysiseDto? _selectedAnalysisForEdit;
    private AnalysiseDto? _selectedAnalysisForDelete;
    private AnalysisDepartmentDto? _selectedAnalysisDepartmentForAnalysis;
    private AnalysisDepartmentDto? _selectedAnalysisDepartmentForEdit;

    private string _newMeasurementName = string.Empty;
    private string _editMeasurementName = string.Empty;
    private MeasurementDto? _selectedMeasurementForEdit;
    private MeasurementDto? _selectedMeasurementForDelete;

    private string _newLpuName = string.Empty;
    private string _newLpuEmail = string.Empty;
    private string _editLpuName = string.Empty;
    private string _editLpuEmail = string.Empty;
    private LpuDto? _selectedLpuForEdit;
    private LpuDto? _selectedLpuForDelete;

    private string _newContractName = string.Empty;
    private int _newContractMoney;
    private double _newContractRemainsMoney;
    private string _editContractName = string.Empty;
    private int _editContractMoney;
    private double _editContractRemainsMoney;
    private ContractDto? _selectedContractForEdit;
    private ContractDto? _selectedContractForDelete;

    private bool _newLpuContractIsActive = true;
    private ContractDto? _selectedContractForLpu;
    private LpuDto? _selectedLpuForContract;
    private LpuContractDto? _selectedLpuContractForDelete;

    private double _newContractAnalysisCost;
    private double _editContractAnalysisCost;
    private ContractDto? _selectedContractForAnalysis;
    private AnalysiseDto? _selectedAnalysisForContract;
    private ContractAnalysisDto? _selectedContractAnalysisForEdit;
    private ContractAnalysisDto? _selectedContractAnalysisForDelete;

    private string? _statusMessage;
    private int _selectedTabIndex;

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
        LpuContracts = new ObservableCollection<LpuContractDto>();
        ContractAnalysisRows = new ObservableCollection<ContractAnalysisDto>();

        LoadDataCommand = ReactiveCommand.CreateFromTask(LoadDataAsync);

        // Role commands
        CreateRoleCommand = ReactiveCommand.CreateFromTask(CreateRoleAsync, this.WhenAnyValue(x => x.NewRoleName).Select(name => !string.IsNullOrWhiteSpace(name)));
        UpdateRoleCommand = ReactiveCommand.CreateFromTask(UpdateRoleAsync, this.WhenAnyValue(x => x.EditRoleName).Select(name => !string.IsNullOrWhiteSpace(name)));
        DeleteRoleCommand = ReactiveCommand.CreateFromTask(DeleteRoleAsync, this.WhenAnyValue(x => x.SelectedRoleForDelete).Select(x => x != null));

        // Worker commands
        CreateWorkerCommand = ReactiveCommand.CreateFromTask(CreateWorkerAsync, this.WhenAnyValue(x => x.NewWorkerName, x => x.NewWorkerLogin, x => x.NewWorkerPassword,
            (name, login, password) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(login) && !string.IsNullOrWhiteSpace(password)));
        UpdateWorkerCommand = ReactiveCommand.CreateFromTask(UpdateWorkerAsync, this.WhenAnyValue(x => x.EditWorkerName, x => x.EditWorkerLogin,
            (name, login) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(login)));
        DeleteWorkerCommand = ReactiveCommand.CreateFromTask(DeleteWorkerAsync, this.WhenAnyValue(x => x.SelectedWorkerForDelete).Select(x => x != null));

        // Material commands
        CreateMaterialCommand = ReactiveCommand.CreateFromTask(CreateMaterialAsync, this.WhenAnyValue(x => x.NewMaterialName).Select(name => !string.IsNullOrWhiteSpace(name)));
        UpdateMaterialCommand = ReactiveCommand.CreateFromTask(UpdateMaterialAsync, this.WhenAnyValue(x => x.EditMaterialName).Select(name => !string.IsNullOrWhiteSpace(name)));
        DeleteMaterialCommand = ReactiveCommand.CreateFromTask(DeleteMaterialAsync, this.WhenAnyValue(x => x.SelectedMaterialForDelete).Select(x => x != null));

        // Department commands
        CreateDepartmentCommand = ReactiveCommand.CreateFromTask(CreateDepartmentAsync, this.WhenAnyValue(x => x.NewDepartmentName).Select(name => !string.IsNullOrWhiteSpace(name)));
        UpdateDepartmentCommand = ReactiveCommand.CreateFromTask(UpdateDepartmentAsync, this.WhenAnyValue(x => x.EditDepartmentName).Select(name => !string.IsNullOrWhiteSpace(name)));
        DeleteDepartmentCommand = ReactiveCommand.CreateFromTask(DeleteDepartmentAsync, this.WhenAnyValue(x => x.SelectedDepartmentForDelete).Select(x => x != null));

        // Analysis commands
        CreateAnalysisCommand = ReactiveCommand.CreateFromTask(CreateAnalysisAsync, this.WhenAnyValue(x => x.NewAnalysisName).Select(name => !string.IsNullOrWhiteSpace(name)));
        UpdateAnalysisCommand = ReactiveCommand.CreateFromTask(UpdateAnalysisAsync, this.WhenAnyValue(x => x.EditAnalysisName).Select(name => !string.IsNullOrWhiteSpace(name)));
        DeleteAnalysisCommand = ReactiveCommand.CreateFromTask(DeleteAnalysisAsync, this.WhenAnyValue(x => x.SelectedAnalysisForDelete).Select(x => x != null));

        // Measurement commands
        CreateMeasurementCommand = ReactiveCommand.CreateFromTask(CreateMeasurementAsync, this.WhenAnyValue(x => x.NewMeasurementName).Select(name => !string.IsNullOrWhiteSpace(name)));
        UpdateMeasurementCommand = ReactiveCommand.CreateFromTask(UpdateMeasurementAsync, this.WhenAnyValue(x => x.EditMeasurementName).Select(name => !string.IsNullOrWhiteSpace(name)));
        DeleteMeasurementCommand = ReactiveCommand.CreateFromTask(DeleteMeasurementAsync, this.WhenAnyValue(x => x.SelectedMeasurementForDelete).Select(x => x != null));

        // LPU commands
        CreateLpuCommand = ReactiveCommand.CreateFromTask(CreateLpuAsync, this.WhenAnyValue(x => x.NewLpuName).Select(name => !string.IsNullOrWhiteSpace(name)));
        UpdateLpuCommand = ReactiveCommand.CreateFromTask(UpdateLpuAsync, this.WhenAnyValue(x => x.EditLpuName).Select(name => !string.IsNullOrWhiteSpace(name)));
        DeleteLpuCommand = ReactiveCommand.CreateFromTask(DeleteLpuAsync, this.WhenAnyValue(x => x.SelectedLpuForDelete).Select(x => x != null));

        // Contract commands
        CreateContractCommand = ReactiveCommand.CreateFromTask(CreateContractAsync, this.WhenAnyValue(x => x.NewContractName).Select(name => !string.IsNullOrWhiteSpace(name)));
        UpdateContractCommand = ReactiveCommand.CreateFromTask(UpdateContractAsync, this.WhenAnyValue(x => x.EditContractName).Select(name => !string.IsNullOrWhiteSpace(name)));
        DeleteContractCommand = ReactiveCommand.CreateFromTask(DeleteContractAsync, this.WhenAnyValue(x => x.SelectedContractForDelete).Select(x => x != null));

        // LPU-Contract commands
        CreateLpuContractCommand = ReactiveCommand.CreateFromTask(CreateLpuContractAsync, this.WhenAnyValue(x => x.SelectedContractForLpu, x => x.SelectedLpuForContract, (contract, lpu) => contract != null && lpu != null));
        DeleteLpuContractCommand = ReactiveCommand.CreateFromTask(DeleteLpuContractAsync, this.WhenAnyValue(x => x.SelectedLpuContractForDelete).Select(x => x != null));

        // Contract-Analysis commands
        CreateContractAnalysisCommand = ReactiveCommand.CreateFromTask(CreateContractAnalysisAsync, this.WhenAnyValue(x => x.SelectedContractForAnalysis, x => x.SelectedAnalysisForContract, (contract, analysis) => contract != null && analysis != null));
        UpdateContractAnalysisCommand = ReactiveCommand.CreateFromTask(UpdateContractAnalysisAsync, this.WhenAnyValue(x => x.SelectedContractAnalysisForEdit).Select(x => x != null));
        DeleteContractAnalysisCommand = ReactiveCommand.CreateFromTask(DeleteContractAnalysisAsync, this.WhenAnyValue(x => x.SelectedContractAnalysisForDelete).Select(x => x != null));

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
    public ObservableCollection<LpuContractDto> LpuContracts { get; }
    public ObservableCollection<ContractAnalysisDto> ContractAnalysisRows { get; }

    public int SelectedTabIndex { get => _selectedTabIndex; set => this.RaiseAndSetIfChanged(ref _selectedTabIndex, value); }

    // New item properties
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

    // Edit item properties
    public string EditRoleName { get => _editRoleName; set => this.RaiseAndSetIfChanged(ref _editRoleName, value); }
    public string EditWorkerName { get => _editWorkerName; set => this.RaiseAndSetIfChanged(ref _editWorkerName, value); }
    public string EditWorkerLogin { get => _editWorkerLogin; set => this.RaiseAndSetIfChanged(ref _editWorkerLogin, value); }
    public string EditWorkerPassword { get => _editWorkerPassword; set => this.RaiseAndSetIfChanged(ref _editWorkerPassword, value); }
    public string EditMaterialName { get => _editMaterialName; set => this.RaiseAndSetIfChanged(ref _editMaterialName, value); }
    public string EditDepartmentName { get => _editDepartmentName; set => this.RaiseAndSetIfChanged(ref _editDepartmentName, value); }
    public string EditAnalysisName { get => _editAnalysisName; set => this.RaiseAndSetIfChanged(ref _editAnalysisName, value); }
    public string EditAnalysisCode { get => _editAnalysisCode; set => this.RaiseAndSetIfChanged(ref _editAnalysisCode, value); }
    public string EditMeasurementName { get => _editMeasurementName; set => this.RaiseAndSetIfChanged(ref _editMeasurementName, value); }
    public string EditLpuName { get => _editLpuName; set => this.RaiseAndSetIfChanged(ref _editLpuName, value); }
    public string EditLpuEmail { get => _editLpuEmail; set => this.RaiseAndSetIfChanged(ref _editLpuEmail, value); }
    public string EditContractName { get => _editContractName; set => this.RaiseAndSetIfChanged(ref _editContractName, value); }
    public int EditContractMoney { get => _editContractMoney; set => this.RaiseAndSetIfChanged(ref _editContractMoney, value); }
    public double EditContractRemainsMoney { get => _editContractRemainsMoney; set => this.RaiseAndSetIfChanged(ref _editContractRemainsMoney, value); }
    public double EditContractAnalysisCost { get => _editContractAnalysisCost; set => this.RaiseAndSetIfChanged(ref _editContractAnalysisCost, value); }

    // Selection properties for edit
    public RoleDto? SelectedRoleForEdit { get => _selectedRoleForEdit; set { this.RaiseAndSetIfChanged(ref _selectedRoleForEdit, value); if (value != null) EditRoleName = value.RoleName; } }
    public RoleDto? SelectedRoleForDelete { get => _selectedRoleForDelete; set => this.RaiseAndSetIfChanged(ref _selectedRoleForDelete, value); }
    public WorkerDto? SelectedWorkerForEdit { get => _selectedWorkerForEdit; set { this.RaiseAndSetIfChanged(ref _selectedWorkerForEdit, value); if (value != null) { EditWorkerName = value.WorkerFio; EditWorkerLogin = value.WorkerLogin; EditWorkerPassword = value.WorkerPassword; } } }
    public WorkerDto? SelectedWorkerForDelete { get => _selectedWorkerForDelete; set => this.RaiseAndSetIfChanged(ref _selectedWorkerForDelete, value); }
    public MaterialDto? SelectedMaterialForEdit { get => _selectedMaterialForEdit; set { this.RaiseAndSetIfChanged(ref _selectedMaterialForEdit, value); if (value != null) EditMaterialName = value.MaterialName; } }
    public MaterialDto? SelectedMaterialForDelete { get => _selectedMaterialForDelete; set => this.RaiseAndSetIfChanged(ref _selectedMaterialForDelete, value); }
    public AnalysisDepartmentDto? SelectedDepartmentForEdit { get => _selectedDepartmentForEdit; set { this.RaiseAndSetIfChanged(ref _selectedDepartmentForEdit, value); if (value != null) EditDepartmentName = value.AnalysisDepName; } }
    public AnalysisDepartmentDto? SelectedDepartmentForDelete { get => _selectedDepartmentForDelete; set => this.RaiseAndSetIfChanged(ref _selectedDepartmentForDelete, value); }
    public AnalysiseDto? SelectedAnalysisForEdit { get => _selectedAnalysisForEdit; set { this.RaiseAndSetIfChanged(ref _selectedAnalysisForEdit, value); if (value != null) { EditAnalysisName = value.AnalysisName; EditAnalysisCode = value.AnalysisCodeName; SelectedAnalysisDepartmentForEdit = Departments.FirstOrDefault(d => d.AnalysisDepId == value.AnalysisDepId); } } }
    public AnalysiseDto? SelectedAnalysisForDelete { get => _selectedAnalysisForDelete; set => this.RaiseAndSetIfChanged(ref _selectedAnalysisForDelete, value); }
    public MeasurementDto? SelectedMeasurementForEdit { get => _selectedMeasurementForEdit; set { this.RaiseAndSetIfChanged(ref _selectedMeasurementForEdit, value); if (value != null) EditMeasurementName = value.MeasurementName; } }
    public MeasurementDto? SelectedMeasurementForDelete { get => _selectedMeasurementForDelete; set => this.RaiseAndSetIfChanged(ref _selectedMeasurementForDelete, value); }
    public LpuDto? SelectedLpuForEdit { get => _selectedLpuForEdit; set { this.RaiseAndSetIfChanged(ref _selectedLpuForEdit, value); if (value != null) { EditLpuName = value.LpuName; } } }
    public LpuDto? SelectedLpuForDelete { get => _selectedLpuForDelete; set => this.RaiseAndSetIfChanged(ref _selectedLpuForDelete, value); }
    public ContractDto? SelectedContractForEdit { get => _selectedContractForEdit; set { this.RaiseAndSetIfChanged(ref _selectedContractForEdit, value); if (value != null) EditContractName = value.ContractName; } }
    public ContractDto? SelectedContractForDelete { get => _selectedContractForDelete; set => this.RaiseAndSetIfChanged(ref _selectedContractForDelete, value); }
    public LpuContractDto? SelectedLpuContractForDelete { get => _selectedLpuContractForDelete; set => this.RaiseAndSetIfChanged(ref _selectedLpuContractForDelete, value); }
    public ContractAnalysisDto? SelectedContractAnalysisForEdit { get => _selectedContractAnalysisForEdit; set { this.RaiseAndSetIfChanged(ref _selectedContractAnalysisForEdit, value); if (value != null) EditContractAnalysisCost = value.ContrAnalysisCost; } }
    public ContractAnalysisDto? SelectedContractAnalysisForDelete { get => _selectedContractAnalysisForDelete; set => this.RaiseAndSetIfChanged(ref _selectedContractAnalysisForDelete, value); }

    public string? StatusMessage { get => _statusMessage; set => this.RaiseAndSetIfChanged(ref _statusMessage, value); }

    public RoleDto? SelectedRoleForWorker { get => _selectedRoleForWorker; set => this.RaiseAndSetIfChanged(ref _selectedRoleForWorker, value); }
    public RoleDto? SelectedRoleForEditWorker { get => _selectedRoleForEditWorker; set => this.RaiseAndSetIfChanged(ref _selectedRoleForEditWorker, value); }
    public AnalysisDepartmentDto? SelectedAnalysisDepartmentForAnalysis { get => _selectedAnalysisDepartmentForAnalysis; set => this.RaiseAndSetIfChanged(ref _selectedAnalysisDepartmentForAnalysis, value); }
    public AnalysisDepartmentDto? SelectedAnalysisDepartmentForEdit { get => _selectedAnalysisDepartmentForEdit; set => this.RaiseAndSetIfChanged(ref _selectedAnalysisDepartmentForEdit, value); }
    public ContractDto? SelectedContractForLpu { get => _selectedContractForLpu; set => this.RaiseAndSetIfChanged(ref _selectedContractForLpu, value); }
    public ContractDto? SelectedContractForAnalysis { get => _selectedContractForAnalysis; set => this.RaiseAndSetIfChanged(ref _selectedContractForAnalysis, value); }
    public AnalysiseDto? SelectedAnalysisForContract { get => _selectedAnalysisForContract; set => this.RaiseAndSetIfChanged(ref _selectedAnalysisForContract, value); }
    public LpuDto? SelectedLpuForContract { get => _selectedLpuForContract; set => this.RaiseAndSetIfChanged(ref _selectedLpuForContract, value); }

    public ReactiveCommand<Unit, Unit> LoadDataCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateRoleCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateRoleCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteRoleCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateWorkerCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateWorkerCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteWorkerCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateMaterialCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateMaterialCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteMaterialCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateDepartmentCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateDepartmentCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteDepartmentCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateAnalysisCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateAnalysisCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteAnalysisCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateMeasurementCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateMeasurementCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteMeasurementCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateLpuCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateLpuCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteLpuCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateContractCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateContractCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteContractCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateLpuContractCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteLpuContractCommand { get; }
    public ReactiveCommand<Unit, Unit> CreateContractAnalysisCommand { get; }
    public ReactiveCommand<Unit, Unit> UpdateContractAnalysisCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteContractAnalysisCommand { get; }

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
            LpuContracts.Clear();
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
            foreach (var lpuContract in await AppServices.Api.GetLpuContractsAsync())
                LpuContracts.Add(lpuContract);

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

    // ========== ROLE ==========
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

    private async Task UpdateRoleAsync()
    {
        if (SelectedRoleForEdit == null) return;
        try
        {
            await AppServices.Api.UpdateRoleAsync(SelectedRoleForEdit.RoleId, EditRoleName);
            SelectedRoleForEdit = null;
            EditRoleName = string.Empty;
            await LoadDataAsync();
            StatusMessage = "Роль обновлена";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task DeleteRoleAsync()
    {
        if (SelectedRoleForDelete == null) return;
        try
        {
            await AppServices.Api.DeleteRoleAsync(SelectedRoleForDelete.RoleId);
            SelectedRoleForDelete = null;
            await LoadDataAsync();
            StatusMessage = "Роль удалена";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    // ========== WORKER ==========
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

    private async Task UpdateWorkerAsync()
    {
        if (SelectedWorkerForEdit == null) return;
        try
        {
            var worker = new WorkerDto
            {
                WorkerFio = EditWorkerName,
                WorkerLogin = EditWorkerLogin,
                WorkerPassword = EditWorkerPassword
            };
            await AppServices.Api.UpdateWorkerAsync(SelectedWorkerForEdit.WorkerId, worker);
            if (SelectedRoleForEditWorker != null)
                await AppServices.Api.UpdateWorkerRolesAsync(SelectedWorkerForEdit.WorkerId, new[] { SelectedRoleForEditWorker.RoleId });

            SelectedWorkerForEdit = null;
            EditWorkerName = string.Empty;
            EditWorkerLogin = string.Empty;
            EditWorkerPassword = string.Empty;
            SelectedRoleForEditWorker = null;
            await LoadDataAsync();
            StatusMessage = "Работник обновлён";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task DeleteWorkerAsync()
    {
        if (SelectedWorkerForDelete == null) return;
        try
        {
            await AppServices.Api.DeleteWorkerAsync(SelectedWorkerForDelete.WorkerId);
            SelectedWorkerForDelete = null;
            await LoadDataAsync();
            StatusMessage = "Работник удалён";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    // ========== MATERIAL ==========
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

    private async Task UpdateMaterialAsync()
    {
        if (SelectedMaterialForEdit == null) return;
        try
        {
            await AppServices.Api.UpdateMaterialAsync(SelectedMaterialForEdit.MaterialId, EditMaterialName);
            SelectedMaterialForEdit = null;
            EditMaterialName = string.Empty;
            await LoadDataAsync();
            StatusMessage = "Материал обновлён";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task DeleteMaterialAsync()
    {
        if (SelectedMaterialForDelete == null) return;
        try
        {
            await AppServices.Api.DeleteMaterialAsync(SelectedMaterialForDelete.MaterialId);
            SelectedMaterialForDelete = null;
            await LoadDataAsync();
            StatusMessage = "Материал удалён";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    // ========== DEPARTMENT ==========
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

    private async Task UpdateDepartmentAsync()
    {
        if (SelectedDepartmentForEdit == null) return;
        try
        {
            await AppServices.Api.UpdateAnalysisDepartmentAsync(SelectedDepartmentForEdit.AnalysisDepId, EditDepartmentName);
            SelectedDepartmentForEdit = null;
            EditDepartmentName = string.Empty;
            await LoadDataAsync();
            StatusMessage = "Отделение обновлено";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task DeleteDepartmentAsync()
    {
        if (SelectedDepartmentForDelete == null) return;
        try
        {
            await AppServices.Api.DeleteAnalysisDepartmentAsync(SelectedDepartmentForDelete.AnalysisDepId);
            SelectedDepartmentForDelete = null;
            await LoadDataAsync();
            StatusMessage = "Отделение удалено";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    // ========== ANALYSIS ==========
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

    private async Task UpdateAnalysisAsync()
    {
        if (SelectedAnalysisForEdit == null) return;
        try
        {
            await AppServices.Api.UpdateAnalysisAsync(SelectedAnalysisForEdit.AnalysisId, EditAnalysisName, SelectedAnalysisDepartmentForEdit?.AnalysisDepId, EditAnalysisCode);
            SelectedAnalysisForEdit = null;
            EditAnalysisName = string.Empty;
            EditAnalysisCode = string.Empty;
            SelectedAnalysisDepartmentForEdit = null;
            await LoadDataAsync();
            StatusMessage = "Анализ обновлён";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task DeleteAnalysisAsync()
    {
        if (SelectedAnalysisForDelete == null) return;
        try
        {
            await AppServices.Api.DeleteAnalysisAsync(SelectedAnalysisForDelete.AnalysisId);
            SelectedAnalysisForDelete = null;
            await LoadDataAsync();
            StatusMessage = "Анализ удалён";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    // ========== MEASUREMENT ==========
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

    private async Task UpdateMeasurementAsync()
    {
        if (SelectedMeasurementForEdit == null) return;
        try
        {
            await AppServices.Api.UpdateMeasurementAsync(SelectedMeasurementForEdit.MeasurementId, EditMeasurementName);
            SelectedMeasurementForEdit = null;
            EditMeasurementName = string.Empty;
            await LoadDataAsync();
            StatusMessage = "Единица измерения обновлена";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task DeleteMeasurementAsync()
    {
        if (SelectedMeasurementForDelete == null) return;
        try
        {
            await AppServices.Api.DeleteMeasurementAsync(SelectedMeasurementForDelete.MeasurementId);
            SelectedMeasurementForDelete = null;
            await LoadDataAsync();
            StatusMessage = "Единица измерения удалена";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    // ========== LPU ==========
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

    private async Task UpdateLpuAsync()
    {
        if (SelectedLpuForEdit == null) return;
        try
        {
            await AppServices.Api.UpdateLpuAsync(SelectedLpuForEdit.LpuId, EditLpuName, string.IsNullOrWhiteSpace(EditLpuEmail) ? null : EditLpuEmail);
            SelectedLpuForEdit = null;
            EditLpuName = string.Empty;
            EditLpuEmail = string.Empty;
            await LoadDataAsync();
            StatusMessage = "ЛПУ обновлено";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task DeleteLpuAsync()
    {
        if (SelectedLpuForDelete == null) return;
        try
        {
            await AppServices.Api.DeleteLpuAsync(SelectedLpuForDelete.LpuId);
            SelectedLpuForDelete = null;
            await LoadDataAsync();
            StatusMessage = "ЛПУ удалено";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    // ========== CONTRACT ==========
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

    private async Task UpdateContractAsync()
    {
        if (SelectedContractForEdit == null) return;
        try
        {
            await AppServices.Api.UpdateContractAsync(SelectedContractForEdit.ContractId, EditContractName, EditContractMoney, EditContractRemainsMoney);
            SelectedContractForEdit = null;
            EditContractName = string.Empty;
            EditContractMoney = 0;
            EditContractRemainsMoney = 0;
            await LoadDataAsync();
            StatusMessage = "Контракт обновлён";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task DeleteContractAsync()
    {
        if (SelectedContractForDelete == null) return;
        try
        {
            await AppServices.Api.DeleteContractAsync(SelectedContractForDelete.ContractId);
            SelectedContractForDelete = null;
            await LoadDataAsync();
            StatusMessage = "Контракт удалён";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    // ========== LPU CONTRACT ==========
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

    private async Task DeleteLpuContractAsync()
    {
        if (SelectedLpuContractForDelete == null) return;
        try
        {
            await AppServices.Api.DeleteLpuContractAsync(SelectedLpuContractForDelete.ConLpuId);
            SelectedLpuContractForDelete = null;
            await LoadDataAsync();
            StatusMessage = "Связь ЛПУ и контракта удалена";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    // ========== CONTRACT ANALYSIS ==========
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

    private async Task UpdateContractAnalysisAsync()
    {
        if (SelectedContractAnalysisForEdit == null) return;
        try
        {
            await AppServices.Api.UpdateContractAnalysisAsync(SelectedContractAnalysisForEdit.ContractId, SelectedContractAnalysisForEdit.AnalysisId, EditContractAnalysisCost);
            SelectedContractAnalysisForEdit = null;
            EditContractAnalysisCost = 0;
            await LoadContractAnalysesAsync();
            StatusMessage = "Стоимость анализа в контракте обновлена";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task DeleteContractAnalysisAsync()
    {
        if (SelectedContractAnalysisForDelete == null) return;
        try
        {
            await AppServices.Api.DeleteContractAnalysisAsync(SelectedContractAnalysisForDelete.ContractId, SelectedContractAnalysisForDelete.AnalysisId);
            SelectedContractAnalysisForDelete = null;
            await LoadContractAnalysesAsync();
            StatusMessage = "Связь анализа и контракта удалена";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }
}