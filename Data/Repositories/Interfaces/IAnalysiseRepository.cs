using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IAnalysiseRepository
{
    List<Analysise> GetAllAnalyses();
    Analysise GetAnalysisByAnalysisId(long analysisId);
    bool CreateAnalysis(Analysise analysis);
    bool DeleteAnalysis(long analysisId);
    Array GetAnalysesByDepartments();
    bool UpdateAnalysis(Analysise analysis);
    bool UpdateDepartmentOfAnalysis(Analysise analysise, AnalysisDepartment analysisDepartment);
}
