using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class QuantitativeStandartRepositoryImpl : IQuantitativeStandartRepository
{
    SystemdatabaseContext _context;
    public QuantitativeStandartRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<QuantitativeStandart> GetAllQuantitativeStandarts()
    {
        return _context.QuantitativeStandarts.ToList();
    }

    public QuantitativeStandart GetQuantitativeStandartByQuantStandartId(int quantStandartId)
    {
        return _context.QuantitativeStandarts.First(x => x.QuantStandartId == quantStandartId);
    }

    public bool CreateQuantitativeStandart(QuantitativeStandart quantitativeStandart)
    {
        _context.QuantitativeStandarts.Add(quantitativeStandart);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateQuantitativeStandart(QuantitativeStandart quantitativeStandart)
    {
        var existing = _context.QuantitativeStandarts.First(x =>
            x.QuantStandartId == quantitativeStandart.QuantStandartId);
        existing.AnalysisWorkId = quantitativeStandart.AnalysisWorkId;
        existing.RefGroupId = quantitativeStandart.RefGroupId;
        existing.QuantStandartLowNorm = quantitativeStandart.QuantStandartLowNorm;
        existing.QuantStandartHighNorm = quantitativeStandart.QuantStandartHighNorm;
        existing.QuantStandartLowPathology = quantitativeStandart.QuantStandartLowPathology;
        existing.QuantStandartHighPathology = quantitativeStandart.QuantStandartHighPathology;
        existing.QuantStandartLowCritical = quantitativeStandart.QuantStandartLowCritical;
        existing.QuantStandartHighCritical = quantitativeStandart.QuantStandartHighCritical;
        existing.QuantStandartDescription = quantitativeStandart.QuantStandartDescription;
        existing.MeasurementsId = quantitativeStandart.MeasurementsId;
        _context.SaveChanges();
        return true;
    }

    public bool UpdateQuantitativeStandartRefGroup(int quantStandartId, int refGroupId)
    {
        var existing = _context.QuantitativeStandarts.First(x => x.QuantStandartId == quantStandartId);
        existing.RefGroupId = refGroupId;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteQuantitativeStandart(int quantStandartId)
    {
        var quantitativeStandart = _context.QuantitativeStandarts.First(x => x.QuantStandartId == quantStandartId);
        _context.QuantitativeStandarts.Remove(quantitativeStandart);
        _context.SaveChanges();
        return true;
    }
}
