namespace Desktop.Models;

public class WorkerDto
{
    public int WorkerId { get; set; }
    public string WorkerFio { get; set; } = string.Empty;
    public string WorkerLogin { get; set; } = string.Empty;
}

public class PatientDto
{
    public long PatientId { get; set; }
    public string PatientFirstName { get; set; } = string.Empty;
    public string? PatientSecondName { get; set; }
    public string? PatientLastName { get; set; }
    public DateOnly? PatientBirthday { get; set; }
    public string PatientGender { get; set; } = string.Empty;

    public string FullName =>
        string.Join(" ", new[] { PatientLastName, PatientFirstName, PatientSecondName }
            .Where(s => !string.IsNullOrWhiteSpace(s)));
}

public class OrderDto
{
    public long OrderId { get; set; }
    public long? DocId { get; set; }
    public string? OrderLpuDepartment { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public long PatientId { get; set; }
    public long LpuId { get; set; }
    public bool OrderIsCountingInContract { get; set; }
    public List<BarcodeMaterialDto>? BarcodeMaterials { get; set; }
    public PatientDto? Patient { get; set; }
    public LpuDto? Lpu { get; set; }
    public DoctorDto? Doc { get; set; }
}

public class LpuDto
{
    public long LpuId { get; set; }
    public string LpuName { get; set; } = string.Empty;
}

public class DoctorDto
{
    public long DocId { get; set; }
    public string DocFio { get; set; } = string.Empty;
    public long? LpuId { get; set; }
}

public class MaterialDto
{
    public int MaterialId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
}

public class AnalysisDepartmentDto
{
    public int AnalysisDepId { get; set; }
    public string AnalysisDepName { get; set; } = string.Empty;
}

public class AnalysiseDto
{
    public long AnalysisId { get; set; }
    public string AnalysisName { get; set; } = string.Empty;
    public int? AnalysisDepId { get; set; }
    public string AnalysisCodeName { get; set; } = string.Empty;
}

public class BarcodeMaterialDto
{
    public decimal BarcodeMatId { get; set; }
    public long? OrderId { get; set; }
    public int? MaterialId { get; set; }
    public int AnalysisDepId { get; set; }
    public MaterialDto? Material { get; set; }
    public AnalysisDepartmentDto? AnalysisDep { get; set; }
    public List<BarcodeAnalysiseDto>? BarcodeAnalysises { get; set; }
}

public class BarcodeAnalysiseDto
{
    public decimal BarcodeId { get; set; }
    public long AnalysisId { get; set; }
    public string? Result { get; set; }
    public int AnalysisDepId { get; set; }
    public AnalysiseDto? Analysis { get; set; }
    public BarcodeMaterialDto? BarcodeMaterial { get; set; }
}

public class TripodDto
{
    public long TripodId { get; set; }
    public string TripodName { get; set; } = string.Empty;
    public DateOnly TripodCreateDate { get; set; }
    public int TripodMaxCell { get; set; }
    public int AnalysisDepartmentId { get; set; }
    public AnalysisDepartmentDto? AnalysisDepartment { get; set; }
}

public class TripodBarcodeMaterialDto
{
    public long TripodId { get; set; }
    public decimal BarcodeMatId { get; set; }
    public int AnalysisDepId { get; set; }
    public BarcodeMaterialDto? BarcodeMaterial { get; set; }
}

public class WorksheetRowDto
{
    public string BiomaterialBarcode { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public string Analyses { get; set; } = string.Empty;
}

public class ContractAnalysisDto
{
    public long ContractId { get; set; }
    public long AnalysisId { get; set; }
    public double ContrAnalysisCost { get; set; }
    public AnalysiseDto? Analysis { get; set; }
}

public class ContractDto
{
    public long ContractId { get; set; }
    public string ContractName { get; set; } = string.Empty;
}

public class AnalysisSelectionItem
{
    public bool IsSelected { get; set; }
    public long AnalysisId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int AnalysisDepId { get; set; }
}

public class OrderMaterialRow
{
    public string MaterialType { get; set; } = string.Empty;
    public string Ids { get; set; } = string.Empty;
    public string TakenDate { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public decimal BarcodeMatId { get; set; }
    public int MaterialId { get; set; }
    public int AnalysisDepId { get; set; }
}

public class OrderSummaryRow
{
    public string Ids { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Cipher { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public long AnalysisId { get; set; }
    public decimal BarcodeMatId { get; set; }
}

public class TrackerRow
{
    public string Comment { get; set; } = string.Empty;
    public string Ids { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Lpu { get; set; } = string.Empty;
    public string MaterialKind { get; set; } = string.Empty;
    public string MaterialType { get; set; } = string.Empty;
    public decimal BarcodeMatId { get; set; }
}

public class ResultRow
{
    public string Ids { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Lpu { get; set; } = string.Empty;
    public string Doctor { get; set; } = string.Empty;
    public string MaterialType { get; set; } = string.Empty;
    public bool HasResult { get; set; }
    public bool IsSent { get; set; }
}

public class ReportFileItem
{
    public string Name { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}

public class ReportServiceRow
{
    public string Cipher { get; set; } = string.Empty;
    public string ServiceCode { get; set; } = string.Empty;
    public string Service { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Price { get; set; } = string.Empty;
}

public class RackCellState
{
    public int Index { get; set; }
    public bool IsOccupied { get; set; }
    public string Label { get; set; } = string.Empty;
    public string MaterialType { get; set; } = string.Empty;
}

public class WorksheetExpandedRow
{
    public string BiomaterialBarcode { get; set; } = string.Empty;
    public string Kind { get; set; } = string.Empty;
    public Dictionary<string, bool> Analyses { get; set; } = [];
}

public class PriorityItem
{
    public string Text { get; set; } = string.Empty;
}

public enum NavSection
{
    Registration,
    Tracker,
    Racks,
    Results,
    Reports,
    Workflows,
    Worksheets
}
