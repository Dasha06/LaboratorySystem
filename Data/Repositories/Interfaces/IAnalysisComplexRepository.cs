using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IAnalysisComplexRepository
{
    List<AnalysisComplex> GetAllAnalysisComplexes();
    AnalysisComplex GetAnalysisComplexByComplexId(int complexId);
    bool CreateAnalysisComplex(AnalysisComplex analysisComplex);
    bool UpdateAnalysisComplex(AnalysisComplex analysisComplex);
    bool DeleteAnalysisComplex(int complexId);
    bool AddAnalysiseToComplex(int complexId, long analysisId);
    bool RemoveAnalysiseFromComplex(int complexId, long analysisId);
    /// <summary>
    /// Варианты материалов по работам анализа для каждого анализа в составе комплекса (например ПЦР).
    /// </summary>
    List<AnalysisWork> GetAnalysisWorksMaterialOptionsForComplex(int complexId);
}
