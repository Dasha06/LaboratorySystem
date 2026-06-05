namespace WebApi.Models.Forms;

public class MaterialForm
{
    public string MaterialName { get; set; } = string.Empty;
}

public class MeasurementForm
{
    public string MeasurementName { get; set; } = string.Empty;
}

public class RoleForm
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}

public class TypeChangeForm
{
    public int TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
}

public class LpuForm
{
    public string LpuName { get; set; } = string.Empty;
    public string? LpuEmail { get; set; }
}

public class SetLpuEmailForm
{
    public string? Email { get; set; }
}

public class ContractForm
{
    public string ContractName { get; set; } = string.Empty;
    public int ContractMoney { get; set; }
    public double ContractRemainsMoney { get; set; }
}

public class UpdateContractMoneyLimitForm
{
    public int ContractMoney { get; set; }
    public double? ContractRemainsMoney { get; set; }
}

public class LpuContractForm
{
    public long ContractId { get; set; }
    public long LpuId { get; set; }
    public bool ConLpuIsActive { get; set; }
}

public class DoctorForm
{
    public string DocFio { get; set; } = string.Empty;
    public long? LpuId { get; set; }
}

public class PatientForm
{
    public string PatientFirstName { get; set; } = string.Empty;
    public string? PatientSecondName { get; set; }
    public string? PatientLastName { get; set; }
    public DateOnly? PatientBirthday { get; set; }
    public string? PatientEmail { get; set; }
    public string PatientGender { get; set; } = string.Empty;
}

public class OrderForm
{
    public long? DocId { get; set; }
    public string? OrderLpuDepartment { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public long PatientId { get; set; }
    
    public DateOnly OrderTakenDate { get; set; }
    public long LpuId { get; set; }
    public bool OrderIsCountingInContract { get; set; }
}

public class AnalysiseForm
{
    public string AnalysisName { get; set; } = string.Empty;
    public int? AnalysisDepId { get; set; }
    public string AnalysisCodeName { get; set; } = string.Empty;
}

public class UpdateAnalysisDepartmentForm
{
    public int AnalysisDepId { get; set; }
}

public class AnalysisDepartmentForm
{
    public int AnalysisDepId { get; set; }
    public string AnalysisDepName { get; set; } = string.Empty;
}

public class AnalysisComplexForm
{
    public string ComplexName { get; set; } = string.Empty;
    public string ComplexCodeName { get; set; } = string.Empty;
    public int AnalysisDepId { get; set; }
    public string? ComplexDescription { get; set; }
}

public class AnalysisWorkForm
{
    public string AnalysisWorkName { get; set; } = string.Empty;
    public int MaterialId { get; set; }
    public long AnalysisId { get; set; }
}

public class AnalysesTemplateForm
{
    public string AnalysisTempName { get; set; } = string.Empty;
    //Id анализов через запятую, например: 1,2,5
    public string? AnalysisIds { get; set; }
}

public class BarcodeMaterialForm
{
    public decimal BarcodeMatId { get; set; }
    public long? OrderId { get; set; }
    public int? MaterialId { get; set; }
    public int AnalysisDepId { get; set; }
}

public class BarcodeAnalysiseForm
{
    public decimal BarcodeId { get; set; }
    public long AnalysisId { get; set; }
    public int AnalysisDepId { get; set; }
    public string? Result { get; set; }
}

public class SetBarcodeResultForm
{
    public string? ResultJson { get; set; }
}

public class BarcodeComplexForm
{
    public decimal BarcodeMatId { get; set; }
    public int ComplexId { get; set; }
    public int AnalysisDepId { get; set; }
}

public class TripodForm
{
    public string TripodName { get; set; } = string.Empty;
    public DateOnly TripodCreateDate { get; set; }
    public int TripodMaxCell { get; set; }
    public int AnalysisDepartmentId { get; set; }
}

public class TripodBarcodeMaterialForm
{
    public long TripodId { get; set; }
    public decimal BarcodeMatId { get; set; }
    public int AnalysisDepId { get; set; }
}

public class ReferentialGroupForm
{
    public string RefGroupName { get; set; } = string.Empty;
    public string? RefGroupGender { get; set; }
    public double? RefGroupLowAge { get; set; }
    public string? RefGroupLowIf { get; set; }
    public double? RefGroupHighAge { get; set; }
    public string? RefGroupHighIf { get; set; }
    public string? RefGroupCondition { get; set; }
}

public class UpdateRefGroupForm
{
    public int RefGroupId { get; set; }
}

public class QualitativeStandartForm
{
    public int RefGroupId { get; set; }
    public long AnalysisWorkId { get; set; }
}

public class QuantitativeStandartForm
{
    public long AnalysisWorkId { get; set; }
    public int RefGroupId { get; set; }
    public double QuantStandartLowNorm { get; set; }
    public double QuantStandartHighNorm { get; set; }
    public double QuantStandartLowPathology { get; set; }
    public double QuantStandartHighPathology { get; set; }
    public double QuantStandartLowCritical { get; set; }
    public double QuantStandartHighCritical { get; set; }
    public string? QuantStandartDescription { get; set; }
    public int MeasurementsId { get; set; }
}

public class QualityParameterForm
{
    public long QualitativeStandartId { get; set; }
    public string QualityCondition { get; set; } = string.Empty;
    public string? QualityDescription { get; set; }
    public string QualityTypeCondition { get; set; } = string.Empty;
}

public class ContractAnalysiseForm
{
    public long ContractId { get; set; }
    public long AnalysisId { get; set; }
    public double ContrAnalysisCost { get; set; }
}

public class ContractComplexForm
{
    public long ContractId { get; set; }
    public int ComplexId { get; set; }
    public double ContractComplexCost { get; set; }
}

public class OrderChangeForm
{
    public long OrderId { get; set; }
    public int WorkerId { get; set; }
    public DateTime OrderChangeTime { get; set; }
    public int TypeId { get; set; }
}

public class PatientChangeForm
{
    public long PatientId { get; set; }
    public int WorkerId { get; set; }
    public DateTime PatientChangeTime { get; set; }
    public int TypeId { get; set; }
}

public class WorkerForm
{
    public string WorkerFio { get; set; } = string.Empty;
    public string WorkerLogin { get; set; } = string.Empty;
    public string WorkerPassword { get; set; } = string.Empty;
}

public class WorkerLoginForm
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UpdateWorkerRolesForm
{
    //Id ролей через запятую, например: 1,2
    public string RoleIds { get; set; } = string.Empty;
}
