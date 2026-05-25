using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class AnalysisesTemplateRepositoryImpl : IAnalysisesTemplateRepository
{
    SystemdatabaseContext _context;
    public AnalysisesTemplateRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<AnalysesTemplate> GetAllAnalysisesTemplates()
    {
        return _context.AnalysesTemplates.ToList();
    }

    public List<Analysise> GetAnalysesFromTemplate(int analysisTempId)
    {
        var result = _context.AnalysesTemplates
            .Include(x => x.Analyses).First(x => x.AnalysisTempId == analysisTempId);
        return result.Analyses.ToList();
    }

    public bool UpdateAnalysisTemplate(AnalysesTemplate analysisesTemplate)
    {
        var oldTemplate = _context.AnalysesTemplates
            .Include(x => x.Analyses)
            .First(x => x.AnalysisTempId == analysisesTemplate.AnalysisTempId);
        oldTemplate.AnalysisTempName = analysisesTemplate.AnalysisTempName;
        oldTemplate.Analyses.Clear();
        
        foreach (var analysis in analysisesTemplate.Analyses)
        {
            oldTemplate.Analyses.Add(analysis);
        }
        
        _context.Update(oldTemplate);
        _context.SaveChanges();
        return true;
    }

    public bool CreateAnalysisesTemplate(AnalysesTemplate analysisesTemplate)
    {
        _context.AnalysesTemplates.Add(analysisesTemplate);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteAnalysisesTemplate(int analysisTempId)
    {
        var analysisesTemplate = _context.AnalysesTemplates.First(x =>
            x.AnalysisTempId == analysisTempId);
        _context.AnalysesTemplates.Remove(analysisesTemplate);
        _context.SaveChanges();
        return true;
    }
}
