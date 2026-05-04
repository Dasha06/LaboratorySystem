using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class AnalysisDepartmentRepositoryImpl : IAnalysisDepartmentRepository
{
    SystemdatabaseContext _context;
    public AnalysisDepartmentRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<AnalysisDepartment> GetAllAnalysisDepartments()
    {
        return _context.AnalysisDepartments.ToList();
    }

    public AnalysisDepartment GetAnalysisDepartmentByAnalysisDepId(int analysisDepId)
    {
        return _context.AnalysisDepartments.First(x => x.AnalysisDepId == analysisDepId);
    }

    public bool CreateAnalysisDepartment(AnalysisDepartment analysisDepartment)
    {
        _context.AnalysisDepartments.Add(analysisDepartment);
        _context.SaveChanges();
        return true;
    }
    // get dep by analysis?

    public bool CreateAnalysisDepartments(List<AnalysisDepartment> analysisDepartments)
    {
        _context.AnalysisDepartments.AddRange(analysisDepartments);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteAnalysisDepartment(int analysisDepId)
    {
        var analysisDepartment = _context.AnalysisDepartments.First(x => x.AnalysisDepId == analysisDepId);
        _context.AnalysisDepartments.Remove(analysisDepartment);
        _context.SaveChanges();
        return true;
    }
}
