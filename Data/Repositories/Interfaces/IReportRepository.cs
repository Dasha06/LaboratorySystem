using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IReportRepository
{
    /// <summary>
    /// 1. Количество созданных заказов по работникам за промежуток времени.
    /// Returns worker FIO and count of orders created (OrderChange with TypeId = 1 = создание).
    /// </summary>
    List<KeyValuePair<string, int>> GetOrdersCountByWorker(DateTime from, DateTime to);

    /// <summary>
    /// 2. Список всех заказанных анализов за промежуток времени.
    /// </summary>
    List<OrderedAnalysisRecord> GetOrderedAnalysesBetween(DateTime from, DateTime to);

    /// <summary>
    /// 3. Список заказанных анализов за промежуток времени по определенному ЛПУ.
    /// </summary>
    List<OrderedAnalysisRecord> GetOrderedAnalysesByLpu(DateTime from, DateTime to, long lpuId);

    /// <summary>
    /// 4. Какие анализы доступны по договорам определенного ЛПУ.
    /// </summary>
    List<AvailableAnalysisRecord> GetAvailableAnalysesByLpu(long lpuId);

    /// <summary>
    /// Список всех ЛПУ для выбора.
    /// </summary>
    List<Lpu> GetAllLpus();
}

public class OrderedAnalysisRecord
{
    public long OrderId { get; set; }
    public string AnalysisName { get; set; } = string.Empty;
    public string AnalysisCodeName { get; set; } = string.Empty;
    public string AnalysisNomenclatureCode { get; set; } = string.Empty;
    public string LpuName { get; set; } = string.Empty;
    public string PatientFio { get; set; } = string.Empty;
    public DateTime OrderChangeTime { get; set; }
}

public class AvailableAnalysisRecord
{
    public long AnalysisId { get; set; }
    public string AnalysisName { get; set; } = string.Empty;
    public string AnalysisCodeName { get; set; } = string.Empty;
    public string AnalysisNomenclatureCode { get; set; } = string.Empty;
    public string ContractName { get; set; } = string.Empty;
    public double ContrAnalysisCost { get; set; }
}