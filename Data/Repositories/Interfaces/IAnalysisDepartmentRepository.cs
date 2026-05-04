using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IAnalysisDepartmentRepository
{
    List<AnalysisDepartment> GetAllAnalysisDepartments();
    AnalysisDepartment GetAnalysisDepartmentByAnalysisDepId(int analysisDepId);
    bool CreateAnalysisDepartment(AnalysisDepartment analysisDepartment);
    bool DeleteAnalysisDepartment(int analysisDepId);
    bool CreateAnalysisDepartments(List<AnalysisDepartment> analysisDepartments);

}
