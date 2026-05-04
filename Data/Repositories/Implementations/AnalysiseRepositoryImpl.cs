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

    public List<Analysise> GetAllAnalysises()
    {
        return _context.Analysises.ToList();
    }

    public Analysise GetAnalysiseByAnalysisId(long analysisId)
    {
        return _context.Analysises.First(x => x.AnalysisId == analysisId);
    }

    public bool CreateAnalysise(Analysise analysise)
    {
        _context.Analysises.Add(analysise);
        _context.SaveChanges();
        return true;
    }

    public bool CreateAnalysises(List<Analysise> analysises)
    {
        _context.Analysises.AddRange(analysises);
        _context.SaveChanges();
        return true;
    }
    
    

    public bool DeleteAnalysise(long analysisId)
    {
        var analysise = _context.Analysises.First(x => x.AnalysisId == analysisId);
        _context.Analysises.Remove(analysise);
        _context.SaveChanges();
        return true;
    }
}
