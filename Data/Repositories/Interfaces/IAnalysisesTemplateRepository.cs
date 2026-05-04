using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IAnalysisesTemplateRepository
{
    List<AnalysisesTemplate> GetAllAnalysisesTemplates();
    AnalysisesTemplate GetAnalysisesTemplateByAnalysisTempIdAndAnalysisId(int analysisTempId, long analysisId);
    bool CreateAnalysisesTemplate(AnalysisesTemplate analysisesTemplate);
    bool DeleteAnalysisesTemplate(int analysisTempId, long analysisId);
}
