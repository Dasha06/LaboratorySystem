using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class ContractRepositoryImpl : IContractRepository
{
    SystemdatabaseContext _context;
    public ContractRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Contract> GetAllContracts()
    {
        return _context.Contracts.ToList();
    }

    public Contract GetContractByContractId(long contractId)
    {
        return _context.Contracts.First(x => x.ContractId == contractId);
    }

    public bool CreateContract(Contract contract)
    {
        _context.Contracts.Add(contract);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteContract(long contractId)
    {
        var contract = _context.Contracts.First(x => x.ContractId == contractId);
        _context.Contracts.Remove(contract);
        _context.SaveChanges();
        return true;
    }
}
