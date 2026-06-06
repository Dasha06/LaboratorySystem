namespace Desktop.Models;

public class WorkerReportRow
{
    public string WorkerFio { get; set; } = string.Empty;
    public int OrdersCount { get; set; }
}

public class OrderedAnalysisReportRow
{
    public long OrderId { get; set; }
    public string AnalysisName { get; set; } = string.Empty;
    public string AnalysisCodeName { get; set; } = string.Empty;
    public string AnalysisNomenclatureCode { get; set; } = string.Empty;
    public string LpuName { get; set; } = string.Empty;
    public string PatientFio { get; set; } = string.Empty;
    public DateTime OrderChangeTime { get; set; }

    // Display helpers for XAML
    public string PatientName => PatientFio;
    public DateTime? OrderDate => OrderChangeTime;
}

public class AvailableAnalysisReportRow
{
    public long AnalysisId { get; set; }
    public string AnalysisName { get; set; } = string.Empty;
    public string AnalysisCodeName { get; set; } = string.Empty;
    public string AnalysisNomenclatureCode { get; set; } = string.Empty;
    public string ContractName { get; set; } = string.Empty;
    public double ContrAnalysisCost { get; set; }

    // Display helper for XAML
    public double Cost => ContrAnalysisCost;
}

public class ReportTypeItem
{
    public string Name { get; set; } = string.Empty;
    public int ReportId { get; set; }
    public bool IsSelected { get; set; }
}