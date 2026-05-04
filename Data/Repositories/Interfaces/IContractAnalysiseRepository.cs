using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IContractAnalysiseRepository
{
    List<ContractAnalysise> GetAllContractAnalysises();
    ContractAnalysise GetContractAnalysiseByContractIdAndAnalysisId(long contractId, long analysisId);
    bool CreateContractAnalysise(ContractAnalysise contractAnalysise);
    bool DeleteContractAnalysise(long contractId, long analysisId);
}
