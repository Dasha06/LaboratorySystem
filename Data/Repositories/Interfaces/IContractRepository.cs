using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IContractRepository
{
    List<Contract> GetAllContracts();
    Contract GetContractByContractId(long contractId);
    bool CreateContract(Contract contract);
    bool UpdateContract(Contract contract);
    bool UpdateContractMoneyLimit(long contractId, int contractMoney, double? contractRemainsMoney = null);
    bool DeleteContract(long contractId);
}
