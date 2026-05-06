using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class AnalysiseRepositoryImpl : IAnalysiseRepository
{
    SystemdatabaseContext _context;
    public AnalysiseRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Analysise> GetAllAnalyses()
    {
        return _context.Analysises.ToList();
    }

    public Array GetAnalysesByDepartments()
    {
        var analysises = _context.Analysises.GroupBy(x => x.AnalysisDep.AnalysisDepName).ToArray();
        return analysises;
    }
    
    public Analysise GetAnalysisByAnalysisId(long analysisId)
    {
        return _context.Analysises.First(x => x.AnalysisId == analysisId);
    }

    public bool CreateAnalysis(Analysise analysis)
    {
        _context.Analysises.Add(analysis);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateAnalysis(Analysise analysis)
    {
        _context.Analysises.Update(analysis);
        _context.SaveChanges();
        return true;
    }
    //is it needed to be in departments?
    public bool UpdateDepartmentOfAnalysis(Analysise analysise, AnalysisDepartment analysisDepartment)
    {
        analysise.AnalysisDepId = analysisDepartment.AnalysisDepId;
        _context.Analysises.Update(analysise);
        _context.SaveChanges();
        return true;
    }
    
    public bool DeleteAnalysis(long analysisId)
    {
        var analysise = _context.Analysises.First(x => x.AnalysisId == analysisId);
        _context.Analysises.Remove(analysise);
        _context.SaveChanges();
        return true;
    }
}
