using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IAnalysiseRepository
{
    List<Analysise> GetAllAnalysises();
    Analysise GetAnalysiseByAnalysisId(long analysisId);
    bool CreateAnalysise(Analysise analysise);
    bool DeleteAnalysise(long analysisId);
    bool CreateAnalysises(List<Analysise> analysises);
}
