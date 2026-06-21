using Data.Models;
using WebApi.Models.Forms;

namespace WebApi.Mapping;

public static class FormEntityMapper
{
    public static Material ToMaterial(MaterialForm form) => new()
    {
        MaterialName = form.MaterialName
    };

    public static Measurement ToMeasurement(MeasurementForm form) => new()
    {
        MeasurementName = form.MeasurementName
    };

    public static Role ToRole(RoleForm form) => new()
    {
        RoleId = form.RoleId,
        RoleName = form.RoleName
    };

    public static TypeChange ToTypeChange(TypeChangeForm form) => new()
    {
        TypeId = form.TypeId,
        TypeName = form.TypeName
    };

    public static Lpu ToLpu(LpuForm form) => new()
    {
        LpuName = form.LpuName,
        LpuEmail = form.LpuEmail
    };

    public static Contract ToContract(ContractForm form) => new()
    {
        ContractName = form.ContractName,
        ContractMoney = form.ContractMoney,
        ContractRemainsMoney = form.ContractRemainsMoney
    };

    public static LpuContract ToLpuContract(LpuContractForm form) => new()
    {
        ContractId = form.ContractId,
        LpuId = form.LpuId,
        ConLpuIsActive = form.ConLpuIsActive
    };

    public static Doctor ToDoctor(DoctorForm form) => new()
    {
        DocFio = form.DocFio,
        LpuId = form.LpuId
    };

    public static Patient ToPatient(PatientForm form) => new()
    {
        PatientFirstName = form.PatientFirstName,
        PatientSecondName = form.PatientSecondName,
        PatientLastName = form.PatientLastName,
        PatientBirthday = form.PatientBirthday,
        PatientEmail = form.PatientEmail,
        PatientGender = form.PatientGender
    };

    public static Order ToOrder(OrderForm form) => new()
    {
        DocId = form.DocId,
        OrderLpuDepartment = form.OrderLpuDepartment,
        OrderStatus = form.OrderStatus,
        OrderTakenDate = form.OrderTakenDate,
        PatientId = form.PatientId,
        LpuId = form.LpuId,
        OrderIsCountingInContract = form.OrderIsCountingInContract
    };

    public static Analysise ToAnalysise(AnalysiseForm form) => new()
    {
        AnalysisName = form.AnalysisName,
        AnalysisDepId = form.AnalysisDepId,
        AnalysisCodeName = form.AnalysisCodeName,
        AnalysisNomenclatureCode = form.AnalysisNomenclatureCode
    };

    public static AnalysisDepartment ToAnalysisDepartment(AnalysisDepartmentForm form) => new()
    {
        AnalysisDepId = form.AnalysisDepId,
        AnalysisDepName = form.AnalysisDepName
    };

    public static AnalysisComplex ToAnalysisComplex(AnalysisComplexForm form) => new()
    {
        ComplexName = form.ComplexName,
        ComplexCodeName = form.ComplexCodeName,
        ComplexNomenclatureCode = form.ComplexNomenclatureCode,
        AnalysisDepId = form.AnalysisDepId,
        ComplexDescription = form.ComplexDescription
    };

    public static AnalysisWork ToAnalysisWork(AnalysisWorkForm form) => new()
    {
        AnalysisWorkName = form.AnalysisWorkName,
        MaterialId = form.MaterialId,
        AnalysisId = form.AnalysisId
    };

    public static BarcodeMaterial ToBarcodeMaterial(BarcodeMaterialForm form) => new()
    {
        BarcodeMatId = form.BarcodeMatId,
        OrderId = form.OrderId,
        MaterialId = form.MaterialId,
        AnalysisDepId = form.AnalysisDepId
    };

    public static BarcodeAnalysise ToBarcodeAnalysise(BarcodeAnalysiseForm form) => new()
    {
        BarcodeId = form.BarcodeId,
        AnalysisId = form.AnalysisId,
        AnalysisDepId = form.AnalysisDepId,
        Result = form.Result
    };

    public static BarcodeComplex ToBarcodeComplex(BarcodeComplexForm form) => new()
    {
        BarcodeMatId = form.BarcodeMatId,
        ComplexId = form.ComplexId,
        AnalysisDepId = form.AnalysisDepId
    };

    public static Tripod ToTripod(TripodForm form) => new()
    {
        TripodName = form.TripodName,
        TripodCreateDate = form.TripodCreateDate,
        TripodMaxCell = form.TripodMaxCell,
        AnalysisDepartmentId = form.AnalysisDepartmentId
    };

    public static TripodBarcodeMaterial ToTripodBarcodeMaterial(TripodBarcodeMaterialForm form) => new()
    {
        TripodId = form.TripodId,
        BarcodeMatId = form.BarcodeMatId,
        AnalysisDepId = form.AnalysisDepId,
        TripodBarcodeMatNumber = form.TripodBarcodeMatNumber
    };

    public static ReferentialGroup ToReferentialGroup(ReferentialGroupForm form) => new()
    {
        RefGroupName = form.RefGroupName,
        RefGroupGender = form.RefGroupGender,
        RefGroupLowAge = form.RefGroupLowAge,
        RefGroupLowIf = form.RefGroupLowIf,
        RefGroupHighAge = form.RefGroupHighAge,
        RefGroupHighIf = form.RefGroupHighIf,
        RefGroupCondition = form.RefGroupCondition
    };

    public static QualitativeStandart ToQualitativeStandart(QualitativeStandartForm form) => new()
    {
        RefGroupId = form.RefGroupId,
        AnalysisWorkId = form.AnalysisWorkId
    };

    public static QuantitativeStandart ToQuantitativeStandart(QuantitativeStandartForm form) => new()
    {
        AnalysisWorkId = form.AnalysisWorkId,
        RefGroupId = form.RefGroupId,
        QuantStandartLowNorm = form.QuantStandartLowNorm,
        QuantStandartHighNorm = form.QuantStandartHighNorm,
        QuantStandartLowPathology = form.QuantStandartLowPathology,
        QuantStandartHighPathology = form.QuantStandartHighPathology,
        QuantStandartLowCritical = form.QuantStandartLowCritical,
        QuantStandartHighCritical = form.QuantStandartHighCritical,
        QuantStandartDescription = form.QuantStandartDescription,
        MeasurementsId = form.MeasurementsId
    };

    public static QualityParameter ToQualityParameter(QualityParameterForm form) => new()
    {
        QualitativeStandartId = form.QualitativeStandartId,
        QualityCondition = form.QualityCondition,
        QualityDescription = form.QualityDescription,
        QualityTypeCondition = form.QualityTypeCondition
    };

    public static ContractAnalysise ToContractAnalysise(ContractAnalysiseForm form) => new()
    {
        ContractId = form.ContractId,
        AnalysisId = form.AnalysisId,
        ContrAnalysisCost = form.ContrAnalysisCost
    };

    public static ContractComplex ToContractComplex(ContractComplexForm form) => new()
    {
        ContractId = form.ContractId,
        ComplexId = form.ComplexId,
        ContractComplexCost = form.ContractComplexCost
    };

    public static OrderChange ToOrderChange(OrderChangeForm form) => new()
    {
        OrderId = form.OrderId,
        WorkerId = form.WorkerId,
        OrderChangeTime = form.OrderChangeTime,
        TypeId = form.TypeId
    };

    public static PatientChange ToPatientChange(PatientChangeForm form) => new()
    {
        PatientId = form.PatientId,
        WorkerId = form.WorkerId,
        PatientChangeTime = form.PatientChangeTime,
        TypeId = form.TypeId
    };

    public static Worker ToWorker(WorkerForm form) => new()
    {
        WorkerFio = form.WorkerFio,
        WorkerLogin = form.WorkerLogin,
        WorkerPassword = form.WorkerPassword
    };

    public static List<long> ParseIds(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
            return [];

        return csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(long.Parse)
            .ToList();
    }

    public static List<Role> ParseRoles(string roleIds) =>
        ParseIds(roleIds).Select(id => new Role { RoleId = (int)id }).ToList();
}
