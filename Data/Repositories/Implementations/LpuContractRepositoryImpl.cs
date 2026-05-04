using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class LpuContractRepositoryImpl : ILpuContractRepository
{
    SystemdatabaseContext _context;
    public LpuContractRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<LpuContract> GetAllLpuContracts()
    {
        return _context.LpuContracts.ToList();
    }

    public LpuContract GetLpuContractByConLpuId(long conLpuId)
    {
        return _context.LpuContracts.First(x => x.ConLpuId == conLpuId);
    }

    public bool CreateLpuContract(LpuContract lpuContract)
    {
        _context.LpuContracts.Add(lpuContract);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteLpuContract(long conLpuId)
    {
        var lpuContract = _context.LpuContracts.First(x => x.ConLpuId == conLpuId);
        _context.LpuContracts.Remove(lpuContract);
        _context.SaveChanges();
        return true;
    }
}
