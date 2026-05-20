using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IContractAnalysiseRepository
{
    List<ContractAnalysise> GetAllContractAnalysises();
    List<ContractAnalysise> GetContractAnalysisesByContractId(long contractId);
    List<Analysise> GetAnalysesAvailableForContract(long contractId);
    ContractAnalysise GetContractAnalysiseByContractIdAndAnalysisId(long contractId, long analysisId);
    bool CreateContractAnalysise(ContractAnalysise contractAnalysise);
    bool UpdateContractAnalysise(ContractAnalysise contractAnalysise);
    bool DeleteContractAnalysise(long contractId, long analysisId);
}
