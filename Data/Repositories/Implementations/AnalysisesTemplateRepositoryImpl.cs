using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class AnalysisesTemplateRepositoryImpl : IAnalysisesTemplateRepository
{
    SystemdatabaseContext _context;
    public AnalysisesTemplateRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<AnalysisesTemplate> GetAllAnalysisesTemplates()
    {
        return _context.AnalysisesTemplates.ToList();
    }

    public AnalysisesTemplate GetAnalysisesTemplateByAnalysisTempIdAndAnalysisId(int analysisTempId, long analysisId)
    {
        return _context.AnalysisesTemplates.First(x => x.AnalysisTempId == analysisTempId && x.AnalysisId == analysisId);
    }

    public bool CreateAnalysisesTemplate(AnalysisesTemplate analysisesTemplate)
    {
        _context.AnalysisesTemplates.Add(analysisesTemplate);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteAnalysisesTemplate(int analysisTempId, long analysisId)
    {
        var analysisesTemplate = _context.AnalysisesTemplates.First(x =>
            x.AnalysisTempId == analysisTempId && x.AnalysisId == analysisId);
        _context.AnalysisesTemplates.Remove(analysisesTemplate);
        _context.SaveChanges();
        return true;
    }
}
