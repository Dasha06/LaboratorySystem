using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class ReportRepositoryImpl : IReportRepository
{
    private readonly SystemdatabaseContext _context;

    public ReportRepositoryImpl(SystemdatabaseContext context)
    {
        _context = context;
    }

    public List<KeyValuePair<string, int>> GetOrdersCountByWorker(DateTime from, DateTime to)
    {
        // TypeId = 1 is "создание" (creation) — assumed based on the data model
        var query = _context.OrderChanges
            .Include(oc => oc.Worker)
            .Where(oc => oc.OrderChangeTime >= from && oc.OrderChangeTime <= to)
            .GroupBy(oc => oc.Worker.WorkerFio)
            .Select(g => new KeyValuePair<string, int>(g.Key, g.Count()))
            .ToList();

        return query;
    }

    public List<OrderedAnalysisRecord> GetOrderedAnalysesBetween(DateTime from, DateTime to)
    {
        var records = _context.OrderChanges
            .Include(oc => oc.Order)
                .ThenInclude(o => o.Lpu)
            .Include(oc => oc.Order)
                .ThenInclude(o => o.Patient)
            .Include(oc => oc.Order)
                .ThenInclude(o => o.BarcodeMaterials)
                .ThenInclude(bm => bm.BarcodeAnalysises)
                .ThenInclude(ba => ba.Analysis)
            .Where(oc => oc.OrderChangeTime >= from && oc.OrderChangeTime <= to)
            .SelectMany(oc => oc.Order.BarcodeMaterials
                .SelectMany(bm => bm.BarcodeAnalysises
                    .Select(ba => new OrderedAnalysisRecord
                    {
                        OrderId = oc.OrderId,
                        AnalysisName = ba.Analysis.AnalysisName,
                        AnalysisCodeName = ba.Analysis.AnalysisCodeName,
                        AnalysisNomenclatureCode = ba.Analysis.AnalysisNomenclatureCode,
                        LpuName = oc.Order.Lpu.LpuName,
                        PatientFio = oc.Order.Patient.PatientFirstName + " " +
                                     oc.Order.Patient.PatientSecondName + " " +
                                     oc.Order.Patient.PatientLastName,
                        OrderChangeTime = oc.OrderChangeTime
                    })))
            .Distinct()
            .ToList();

        return records;
    }

    public List<OrderedAnalysisRecord> GetOrderedAnalysesByLpu(DateTime from, DateTime to, long lpuId)
    {
        var records = _context.OrderChanges
            .Include(oc => oc.Order)
                .ThenInclude(o => o.Lpu)
            .Include(oc => oc.Order)
                .ThenInclude(o => o.Patient)
            .Include(oc => oc.Order)
                .ThenInclude(o => o.BarcodeMaterials)
                .ThenInclude(bm => bm.BarcodeAnalysises)
                .ThenInclude(ba => ba.Analysis)
            .Where(oc => oc.OrderChangeTime >= from && oc.OrderChangeTime <= to && oc.Order.LpuId == lpuId)
            .SelectMany(oc => oc.Order.BarcodeMaterials
                .SelectMany(bm => bm.BarcodeAnalysises
                    .Select(ba => new OrderedAnalysisRecord
                    {
                        OrderId = oc.OrderId,
                        AnalysisName = ba.Analysis.AnalysisName,
                        AnalysisCodeName = ba.Analysis.AnalysisCodeName,
                        AnalysisNomenclatureCode = ba.Analysis.AnalysisNomenclatureCode,
                        LpuName = oc.Order.Lpu.LpuName,
                        PatientFio = oc.Order.Patient.PatientFirstName + " " +
                                     oc.Order.Patient.PatientSecondName + " " +
                                     oc.Order.Patient.PatientLastName,
                        OrderChangeTime = oc.OrderChangeTime
                    })))
            .Distinct()
            .ToList();

        return records;
    }

    public List<AvailableAnalysisRecord> GetAvailableAnalysesByLpu(long lpuId)
    {
        var records = _context.LpuContracts
            .Include(lc => lc.Contract)
                .ThenInclude(c => c.ContractAnalysises)
                .ThenInclude(ca => ca.Analysis)
            .Where(lc => lc.LpuId == lpuId && lc.ConLpuIsActive)
            .SelectMany(lc => lc.Contract.ContractAnalysises
                .Select(ca => new AvailableAnalysisRecord
                {
                    AnalysisId = ca.AnalysisId,
                    AnalysisName = ca.Analysis.AnalysisName,
                    AnalysisCodeName = ca.Analysis.AnalysisCodeName,
                    AnalysisNomenclatureCode = ca.Analysis.AnalysisNomenclatureCode,
                    ContractName = lc.Contract.ContractName,
                    ContrAnalysisCost = ca.ContrAnalysisCost
                }))
            .Distinct()
            .ToList();

        return records;
    }

    public List<Lpu> GetAllLpus()
    {
        return _context.Lpus.ToList();
    }
}