using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class ContractAnalysiseRepositoryImpl : IContractAnalysiseRepository
{
    SystemdatabaseContext _context;
    public ContractAnalysiseRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<ContractAnalysise> GetAllContractAnalysises()
    {
        return _context.ContractAnalysises.ToList();
    }

    public ContractAnalysise GetContractAnalysiseByContractIdAndAnalysisId(long contractId, long analysisId)
    {
        return _context.ContractAnalysises.First(x => x.ContractId == contractId && x.AnalysisId == analysisId);
    }

    public bool CreateContractAnalysise(ContractAnalysise contractAnalysise)
    {
        _context.ContractAnalysises.Add(contractAnalysise);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteContractAnalysise(long contractId, long analysisId)
    {
        var contractAnalysise = _context.ContractAnalysises.First(x => x.ContractId == contractId && x.AnalysisId == analysisId);
        _context.ContractAnalysises.Remove(contractAnalysise);
        _context.SaveChanges();
        return true;
    }
}
