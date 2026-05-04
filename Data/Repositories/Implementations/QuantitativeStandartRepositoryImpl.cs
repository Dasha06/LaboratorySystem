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

    public bool DeleteQuantitativeStandart(int quantStandartId)
    {
        var quantitativeStandart = _context.QuantitativeStandarts.First(x => x.QuantStandartId == quantStandartId);
        _context.QuantitativeStandarts.Remove(quantitativeStandart);
        _context.SaveChanges();
        return true;
    }
}
