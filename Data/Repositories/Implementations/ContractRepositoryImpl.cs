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

    public bool UpdateContract(Contract contract)
    {
        var existing = _context.Contracts.First(x => x.ContractId == contract.ContractId);
        existing.ContractName = contract.ContractName;
        existing.ContractMoney = contract.ContractMoney;
        existing.ContractRemainsMoney = contract.ContractRemainsMoney;
        _context.SaveChanges();
        return true;
    }

    public bool UpdateContractMoneyLimit(long contractId, int contractMoney, double? contractRemainsMoney = null)
    {
        var existing = _context.Contracts.First(x => x.ContractId == contractId);
        existing.ContractMoney = contractMoney;
        if (contractRemainsMoney.HasValue)
            existing.ContractRemainsMoney = contractRemainsMoney.Value;
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
