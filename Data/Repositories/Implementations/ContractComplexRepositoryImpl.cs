using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class ContractComplexRepositoryImpl : IContractComplexRepository
{
    private readonly SystemdatabaseContext _context;

    public ContractComplexRepositoryImpl(SystemdatabaseContext context)
    {
        _context = context;
    }

    public List<ContractComplex> GetAllContractComplexes()
    {
        return _context.ContractComplexes
            .Include(cc => cc.Complex)
            .Include(cc => cc.Contract)
            .ToList();
    }

    public List<ContractComplex> GetContractComplexesByContractId(long contractId)
    {
        return _context.ContractComplexes
            .Include(cc => cc.Complex)
            .Where(cc => cc.ContractId == contractId)
            .ToList();
    }

    public ContractComplex GetContractComplexByContractIdAndComplexId(long contractId, int complexId)
    {
        return _context.ContractComplexes
            .Include(cc => cc.Complex)
            .Include(cc => cc.Contract)
            .First(cc => cc.ContractId == contractId && cc.ComplexId == complexId);
    }

    public bool CreateContractComplex(ContractComplex contractComplex)
    {
        _context.ContractComplexes.Add(contractComplex);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateContractComplex(ContractComplex contractComplex)
    {
        var existing = _context.ContractComplexes.First(cc =>
            cc.ContractId == contractComplex.ContractId && cc.ComplexId == contractComplex.ComplexId);
        existing.ContractComplexCost = contractComplex.ContractComplexCost;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteContractComplex(long contractId, int complexId)
    {
        var entity = _context.ContractComplexes.First(cc =>
            cc.ContractId == contractId && cc.ComplexId == complexId);
        _context.ContractComplexes.Remove(entity);
        _context.SaveChanges();
        return true;
    }
}
