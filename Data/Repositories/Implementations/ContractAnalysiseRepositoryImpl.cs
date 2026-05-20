using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public List<ContractAnalysise> GetContractAnalysisesByContractId(long contractId)
    {
        return _context.ContractAnalysises
            .Include(x => x.Analysis)
            .Where(x => x.ContractId == contractId)
            .ToList();
    }

    public List<Analysise> GetAnalysesAvailableForContract(long contractId)
    {
        var linked = _context.ContractAnalysises.Where(x => x.ContractId == contractId).Select(x => x.AnalysisId)
            .ToHashSet();
        return _context.Analysises.Where(a => !linked.Contains(a.AnalysisId)).ToList();
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

    public bool UpdateContractAnalysise(ContractAnalysise contractAnalysise)
    {
        var existing = _context.ContractAnalysises.First(x =>
            x.ContractId == contractAnalysise.ContractId && x.AnalysisId == contractAnalysise.AnalysisId);
        existing.ContrAnalysisCost = contractAnalysise.ContrAnalysisCost;
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
