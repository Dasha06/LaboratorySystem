using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IAnalysisDepartmentRepository
{
    List<AnalysisDepartment> GetAllAnalysisDepartments();
    AnalysisDepartment GetAnalysisDepartmentByAnalysisDepId(int analysisDepId);
    bool CreateAnalysisDepartment(AnalysisDepartment analysisDepartment);
    bool UpdateAnalysisDepartment(AnalysisDepartment analysisDepartment);
    bool DeleteAnalysisDepartment(int analysisDepId);

}
