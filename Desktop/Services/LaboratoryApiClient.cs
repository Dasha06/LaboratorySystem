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
