using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public List<LpuContract> GetLpuContractsByLpuId(long lpuId)
    {
        return _context.LpuContracts
            .Include(x => x.Contract)
            .Where(x => x.LpuId == lpuId)
            .ToList();
    }

    public List<LpuContract> GetLpuContractsByContractId(long contractId)
    {
        return _context.LpuContracts
            .Include(x => x.Lpu)
            .Where(x => x.ContractId == contractId)
            .ToList();
    }

    public bool CreateLpuContract(LpuContract lpuContract)
    {
        _context.LpuContracts.Add(lpuContract);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateLpuContract(LpuContract lpuContract)
    {
        var existing = _context.LpuContracts.First(x => x.ConLpuId == lpuContract.ConLpuId);
        existing.LpuId = lpuContract.LpuId;
        existing.ContractId = lpuContract.ContractId;
        existing.ConLpuIsActive = lpuContract.ConLpuIsActive;
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
