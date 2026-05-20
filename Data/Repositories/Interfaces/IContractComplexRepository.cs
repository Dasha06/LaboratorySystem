using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IContractComplexRepository
{
    List<ContractComplex> GetAllContractComplexes();
    List<ContractComplex> GetContractComplexesByContractId(long contractId);
    ContractComplex GetContractComplexByContractIdAndComplexId(long contractId, int complexId);
    bool CreateContractComplex(ContractComplex contractComplex);
    bool UpdateContractComplex(ContractComplex contractComplex);
    bool DeleteContractComplex(long contractId, int complexId);
}
