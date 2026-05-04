using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IAnalysisWorkRepository
{
    List<AnalysisWork> GetAllAnalysisWorks();
    AnalysisWork GetAnalysisWorkByAnalysisWorkId(int analysisWorkId);
    bool CreateAnalysisWork(AnalysisWork analysisWork);
    bool DeleteAnalysisWork(int analysisWorkId);
}
