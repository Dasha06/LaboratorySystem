using System.Net.Http.Json;
using System.Text.Json;
using Desktop.Models;

namespace Desktop.Services;

public class LaboratoryApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _http;

    public LaboratoryApiClient(HttpClient http) => _http = http;

    public async Task<WorkerDto> LoginAsync(string login, string password, CancellationToken ct = default)
    {
        var url = $"api/Workers/login?login={Uri.EscapeDataString(login)}&password={Uri.EscapeDataString(password)}";
        var worker = await _http.GetFromJsonAsync<WorkerDto>(url, JsonOptions, ct)
                     ?? throw new InvalidOperationException("Неверный логин или пароль");
        return worker;
    }

    public async Task<List<PatientDto>> GetPatientsAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<PatientDto>>("api/Patients", JsonOptions, ct) ?? [];

    public async Task<PatientDto> CreatePatientAsync(PatientDto patient, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(patient.PatientFirstName), "PatientFirstName" },
            { new StringContent(patient.PatientGender), "PatientGender" }
        };
        if (!string.IsNullOrEmpty(patient.PatientSecondName))
            content.Add(new StringContent(patient.PatientSecondName), "PatientSecondName");
        if (!string.IsNullOrEmpty(patient.PatientLastName))
            content.Add(new StringContent(patient.PatientLastName), "PatientLastName");
        if (patient.PatientBirthday.HasValue)
            content.Add(new StringContent(patient.PatientBirthday.Value.ToString("O")), "PatientBirthday");

        var response = await _http.PostAsync("api/Patients", content, ct);
        response.EnsureSuccessStatusCode();
        return patient;
    }

    public async Task UpdatePatientAsync(long patientId, PatientDto patient, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(patient.PatientFirstName ?? string.Empty), "PatientFirstName" },
            { new StringContent(patient.PatientGender ?? string.Empty), "PatientGender" }
        };
        if (!string.IsNullOrEmpty(patient.PatientSecondName))
            content.Add(new StringContent(patient.PatientSecondName), "PatientSecondName");
        if (!string.IsNullOrEmpty(patient.PatientLastName))
            content.Add(new StringContent(patient.PatientLastName), "PatientLastName");
        if (patient.PatientBirthday.HasValue)
            content.Add(new StringContent(patient.PatientBirthday.Value.ToString("O")), "PatientBirthday");

        var response = await _http.PutAsync($"api/Patients/{patientId}", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<OrderDto>> GetOrdersAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<OrderDto>>("api/Orders", JsonOptions, ct) ?? [];

    public async Task<OrderDto> GetOrderDetailsAsync(long orderId, CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<OrderDto>($"api/Orders/{orderId}/details", JsonOptions, ct)
        ?? throw new InvalidOperationException("Заказ не найден");

    public async Task<OrderDto> CreateOrderAsync(OrderDto order, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(order.OrderStatus), "OrderStatus" },
            { new StringContent(order.PatientId.ToString()), "PatientId" },
            { new StringContent(order.LpuId.ToString()), "LpuId" },
            { new StringContent(order.OrderIsCountingInContract.ToString()), "OrderIsCountingInContract" }
        };
        if (order.DocId.HasValue)
            content.Add(new StringContent(order.DocId.Value.ToString()), "DocId");
        if (!string.IsNullOrEmpty(order.OrderLpuDepartment))
            content.Add(new StringContent(order.OrderLpuDepartment), "OrderLpuDepartment");

        var response = await _http.PostAsync("api/Orders", content, ct);
        response.EnsureSuccessStatusCode();
        return order;
    }

    public async Task UpdateOrderAsync(long orderId, OrderDto order, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(order.OrderStatus ?? string.Empty), "OrderStatus" },
            { new StringContent(order.PatientId.ToString()), "PatientId" },
            { new StringContent(order.LpuId.ToString()), "LpuId" },
            { new StringContent(order.OrderIsCountingInContract.ToString()), "OrderIsCountingInContract" }
        };
        if (order.DocId.HasValue)
            content.Add(new StringContent(order.DocId.Value.ToString()), "DocId");
        if (!string.IsNullOrEmpty(order.OrderLpuDepartment))
            content.Add(new StringContent(order.OrderLpuDepartment), "OrderLpuDepartment");

        var response = await _http.PutAsync($"api/Orders/{orderId}", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<LpuDto>> GetLpusAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<LpuDto>>("api/Lpus", JsonOptions, ct) ?? [];

    public async Task<List<DoctorDto>> GetDoctorsAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<DoctorDto>>("api/Doctors", JsonOptions, ct) ?? [];

    public async Task<List<MaterialDto>> GetMaterialsAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<MaterialDto>>("api/Materials", JsonOptions, ct) ?? [];

    public async Task<List<AnalysisDepartmentDto>> GetAnalysisDepartmentsAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<AnalysisDepartmentDto>>("api/AnalysisDepartments", JsonOptions, ct) ?? [];

    public async Task<List<AnalysiseDto>> GetAnalysesAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<AnalysiseDto>>("api/Analyses", JsonOptions, ct) ?? [];

    public async Task<List<LpuContractDto>> GetLpuContractsAsync(long lpuId, CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<LpuContractDto>>($"api/Lpus/{lpuId}/contracts", JsonOptions, ct) ?? [];

    public async Task<List<RoleDto>> GetRolesAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<RoleDto>>("api/Roles", JsonOptions, ct) ?? [];

    public async Task<List<WorkerDto>> GetWorkersAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<WorkerDto>>("api/Workers", JsonOptions, ct) ?? [];

    public async Task CreateRoleAsync(string roleName, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(roleName), "RoleName" }
        };
        var response = await _http.PostAsync("api/Roles", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateWorkerAsync(WorkerDto worker, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(worker.WorkerFio), "WorkerFio" },
            { new StringContent(worker.WorkerLogin), "WorkerLogin" }
        };
        if (!string.IsNullOrEmpty(worker.WorkerPassword))
            content.Add(new StringContent(worker.WorkerPassword), "WorkerPassword");

        var response = await _http.PostAsync("api/Workers", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateMaterialAsync(string materialName, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(materialName), "MaterialName" }
        };
        var response = await _http.PostAsync("api/Materials", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateWorkerRolesAsync(int workerId, IEnumerable<int> roleIds, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(string.Join(",", roleIds)), "RoleIds" }
        };
        var response = await _http.PutAsync($"api/Workers/{workerId}/roles", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<MeasurementDto>> GetMeasurementsAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<MeasurementDto>>("api/Measurements", JsonOptions, ct) ?? [];

    public async Task CreateMeasurementAsync(string measurementName, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(measurementName), "MeasurementName" }
        };
        var response = await _http.PostAsync("api/Measurements", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateAnalysisDepartmentAsync(string name, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(name), "AnalysisDepName" }
        };
        var response = await _http.PostAsync("api/AnalysisDepartments", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateAnalysisAsync(string name, int? departmentId, string codeName, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(name), "AnalysisName" },
            { new StringContent(codeName), "AnalysisCodeName" }
        };
        if (departmentId.HasValue)
            content.Add(new StringContent(departmentId.Value.ToString()), "AnalysisDepId");

        var response = await _http.PostAsync("api/Analyses", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateLpuAsync(string lpuName, string? email = null, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(lpuName), "LpuName" }
        };
        if (!string.IsNullOrWhiteSpace(email))
            content.Add(new StringContent(email), "LpuEmail");

        var response = await _http.PostAsync("api/Lpus", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateContractAsync(string contractName, int money, double remainsMoney, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(contractName), "ContractName" },
            { new StringContent(money.ToString()), "ContractMoney" },
            { new StringContent(remainsMoney.ToString()), "ContractRemainsMoney" }
        };

        var response = await _http.PostAsync("api/Contracts", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateLpuContractAsync(long contractId, long lpuId, bool isActive, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(contractId.ToString()), "ContractId" },
            { new StringContent(lpuId.ToString()), "LpuId" },
            { new StringContent(isActive.ToString()), "ConLpuIsActive" }
        };

        var response = await _http.PostAsync("api/LpuContracts", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ContractAnalysisDto>> GetContractAnalysesByContractAsync(long contractId, CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<ContractAnalysisDto>>($"api/ContractAnalyses/by-contract/{contractId}", JsonOptions, ct) ?? [];

    public async Task CreateContractAnalysisAsync(long contractId, long analysisId, double cost, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(contractId.ToString()), "ContractId" },
            { new StringContent(analysisId.ToString()), "AnalysisId" },
            { new StringContent(cost.ToString()), "ContrAnalysisCost" }
        };

        var response = await _http.PostAsync("api/ContractAnalyses", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<BarcodeMaterialDto>> GetBarcodeMaterialsAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<BarcodeMaterialDto>>("api/BarcodeMaterials", JsonOptions, ct) ?? [];

    public async Task<List<BarcodeMaterialDto>> GetBarcodeMaterialsByOrderAsync(long orderId, CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<BarcodeMaterialDto>>($"api/BarcodeMaterials/by-order/{orderId}", JsonOptions, ct) ?? [];

    public async Task CreateBarcodeMaterialAsync(BarcodeMaterialDto bm, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(bm.BarcodeMatId.ToString()), "BarcodeMatId" },
            { new StringContent(bm.AnalysisDepId.ToString()), "AnalysisDepId" }
        };
        if (bm.OrderId.HasValue)
            content.Add(new StringContent(bm.OrderId.Value.ToString()), "OrderId");
        if (bm.MaterialId.HasValue)
            content.Add(new StringContent(bm.MaterialId.Value.ToString()), "MaterialId");

        var response = await _http.PostAsync("api/BarcodeMaterials", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateBarcodeAnalysisAsync(BarcodeAnalysiseDto ba, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(ba.BarcodeId.ToString()), "BarcodeId" },
            { new StringContent(ba.AnalysisId.ToString()), "AnalysisId" },
            { new StringContent(ba.AnalysisDepId.ToString()), "AnalysisDepId" }
        };
        var response = await _http.PostAsync("api/BarcodeAnalyses", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<BarcodeAnalysiseDto>> GetBarcodeAnalysesAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<BarcodeAnalysiseDto>>("api/BarcodeAnalyses", JsonOptions, ct) ?? [];

    public async Task<List<BarcodeAnalysiseDto>> GetBarcodeAnalysesByBarcodeAsync(decimal barcodeId, CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<BarcodeAnalysiseDto>>($"api/BarcodeAnalyses/by-barcode/{barcodeId}", JsonOptions, ct) ?? [];

    public async Task<List<TripodDto>> GetTripodsAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<TripodDto>>("api/Tripods", JsonOptions, ct) ?? [];

    public async Task CreateTripodAsync(string name, int maxCell, int analysisDepartmentId, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(name), "TripodName" },
            { new StringContent(DateOnly.FromDateTime(DateTime.UtcNow).ToString("O")), "TripodCreateDate" },
            { new StringContent(maxCell.ToString()), "TripodMaxCell" },
            { new StringContent(analysisDepartmentId.ToString()), "AnalysisDepartmentId" }
        };

        var response = await _http.PostAsync("api/Tripods", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateTripodAsync(long tripodId, string name, int maxCell, int analysisDepartmentId, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(name), "TripodName" },
            { new StringContent(DateOnly.FromDateTime(DateTime.UtcNow).ToString("O")), "TripodCreateDate" },
            { new StringContent(maxCell.ToString()), "TripodMaxCell" },
            { new StringContent(analysisDepartmentId.ToString()), "AnalysisDepartmentId" }
        };

        var response = await _http.PutAsync($"api/Tripods/{tripodId}", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<TripodBarcodeMaterialDto>> GetTripodBarcodeMaterialsByTripodAsync(long tripodId, CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<TripodBarcodeMaterialDto>>($"api/TripodBarcodeMaterials/by-tripod/{tripodId}", JsonOptions, ct) ?? [];

    public async Task<List<WorksheetRowDto>> GetWorksheetsAsync(long tripodId, CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<WorksheetRowDto>>($"api/Tripods/{tripodId}/worksheets", JsonOptions, ct) ?? [];

    public async Task CreateTripodBarcodeMaterialAsync(long tripodId, decimal barcodeMatId, int analysisDepId, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent
        {
            { new StringContent(tripodId.ToString()), "TripodId" },
            { new StringContent(barcodeMatId.ToString()), "BarcodeMatId" },
            { new StringContent(analysisDepId.ToString()), "AnalysisDepId" }
        };
        var response = await _http.PostAsync("api/TripodBarcodeMaterials", content, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<ContractDto>> GetContractsAsync(CancellationToken ct = default) =>
        await _http.GetFromJsonAsync<List<ContractDto>>("api/Contracts", JsonOptions, ct) ?? [];
}
