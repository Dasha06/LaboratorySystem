using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IAnalysisesTemplateRepository
{
    List<AnalysesTemplate> GetAllAnalysisesTemplates();
    bool CreateAnalysisesTemplate(AnalysesTemplate analysisesTemplate);
    bool DeleteAnalysisesTemplate(int analysisTempId);
    List<Analysise> GetAnalysesFromTemplate(int analysisTempId);
    bool UpdateAnalysisTemplate(AnalysesTemplate analysisesTemplate);
}
