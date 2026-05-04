using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IContractRepository
{
    List<Contract> GetAllContracts();
    Contract GetContractByContractId(long contractId);
    bool CreateContract(Contract contract);
    bool DeleteContract(long contractId);
}
