using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IAnalysisWorkRepository
{
    List<AnalysisWork> GetAllAnalysisWorks();
    AnalysisWork GetAnalysisWorkByAnalysisWorkId(long analysisWorkId);
    List<AnalysisWork> GetAnalysisWorkByAnalysis(Analysise analysis);
    bool CreateAnalysisWork(AnalysisWork analysisWork);
    bool UpdateAnalysisWork(AnalysisWork analysisWork);
    bool DeleteAnalysisWork(long analysisWorkId);
}
