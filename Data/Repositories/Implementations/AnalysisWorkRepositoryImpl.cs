using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class AnalysisWorkRepositoryImpl : IAnalysisWorkRepository
{
    SystemdatabaseContext _context;
    public AnalysisWorkRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<AnalysisWork> GetAllAnalysisWorks()
    {
        return _context.AnalysisWorks.ToList();
    }

    public AnalysisWork GetAnalysisWorkByAnalysisWorkId(int analysisWorkId)
    {
        return _context.AnalysisWorks.First(x => x.AnalysisWorkId == analysisWorkId);
    }

    public List<AnalysisWork> GetAnalysisWorkByAnalysis(Analysise analysis)
    {
        return _context.AnalysisWorks.Where(x => x.AnalysisId == analysis.AnalysisId).ToList();
    }

    public bool CreateAnalysisWork(AnalysisWork analysisWork)
    {
        _context.AnalysisWorks.Add(analysisWork);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteAnalysisWork(int analysisWorkId)
    {
        var analysisWork = _context.AnalysisWorks.First(x => x.AnalysisWorkId == analysisWorkId);
        _context.AnalysisWorks.Remove(analysisWork);
        _context.SaveChanges();
        return true;
    }
}
