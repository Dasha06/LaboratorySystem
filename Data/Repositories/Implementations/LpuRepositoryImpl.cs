using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class LpuRepositoryImpl : ILpuRepository
{
    SystemdatabaseContext _context;
    public LpuRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Lpu> GetAllLpus()
    {
        return _context.Lpus.ToList();
    }

    public Lpu GetLpuByLpuId(long lpuId)
    {
        return _context.Lpus.First(x => x.LpuId == lpuId);
    }

    public bool CreateLpu(Lpu lpu)
    {
        _context.Lpus.Add(lpu);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteLpu(long lpuId)
    {
        var lpu = _context.Lpus.First(x => x.LpuId == lpuId);
        _context.Lpus.Remove(lpu);
        _context.SaveChanges();
        return true;
    }
}
