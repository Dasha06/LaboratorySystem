using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class AnalysisComplexRepositoryImpl : IAnalysisComplexRepository
{
    private readonly SystemdatabaseContext _context;

    public AnalysisComplexRepositoryImpl(SystemdatabaseContext context)
    {
        _context = context;
    }

    public List<AnalysisComplex> GetAllAnalysisComplexes()
    {
        return _context.AnalysisComplexes
            .Include(c => c.AnalysisDep)
            .Include(c => c.Analyses)
            .ToList();
    }

    public AnalysisComplex GetAnalysisComplexByComplexId(int complexId)
    {
        return _context.AnalysisComplexes
            .Include(c => c.AnalysisDep)
            .Include(c => c.Analyses)
            .First(c => c.ComplexId == complexId);
    }

    public bool CreateAnalysisComplex(AnalysisComplex analysisComplex)
    {
        _context.AnalysisComplexes.Add(analysisComplex);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateAnalysisComplex(AnalysisComplex analysisComplex)
    {
        var existing = _context.AnalysisComplexes.First(c => c.ComplexId == analysisComplex.ComplexId);
        existing.ComplexName = analysisComplex.ComplexName;
        existing.ComplexCodeName = analysisComplex.ComplexCodeName;
        existing.ComplexDescription = analysisComplex.ComplexDescription;
        existing.AnalysisDepId = analysisComplex.AnalysisDepId;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteAnalysisComplex(int complexId)
    {
        var entity = _context.AnalysisComplexes
            .Include(c => c.Analyses)
            .First(c => c.ComplexId == complexId);
        entity.Analyses.Clear(); // удаляет связь
        _context.AnalysisComplexes.Remove(entity);
        _context.SaveChanges();
        return true;
    }

    public bool AddAnalysiseToComplex(int complexId, long analysisId)
    {
        var complex = _context.AnalysisComplexes
            .Include(c => c.Analyses)
            .First(c => c.ComplexId == complexId);
        var analysis = _context.Analysises.First(a => a.AnalysisId == analysisId);
        if (!complex.Analyses.Any(a => a.AnalysisId == analysisId))
            complex.Analyses.Add(analysis);
        _context.SaveChanges();
        return true;
    }

    public bool RemoveAnalysiseFromComplex(int complexId, long analysisId)
    {
        var complex = _context.AnalysisComplexes
            .Include(c => c.Analyses)
            .First(c => c.ComplexId == complexId);
        var analysis = complex.Analyses.FirstOrDefault(a => a.AnalysisId == analysisId);
        if (analysis != null)
        {
            complex.Analyses.Remove(analysis);
            _context.SaveChanges();
        }

        return true;
    }

    public List<AnalysisWork> GetAnalysisWorksMaterialOptionsForComplex(int complexId)
    {
        var analysisIds = _context.AnalysisComplexes
            .Include(c => c.Analyses)
            .AsNoTracking()
            .First(c => c.ComplexId == complexId)
            .Analyses.Select(a => a.AnalysisId)
            .ToList();

        return _context.AnalysisWorks
            .Include(w => w.Material)
            .Where(w => analysisIds.Contains(w.AnalysisId))
            .ToList();
    }
}
