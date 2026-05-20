using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class QualitativeStandartRepositoryImpl : IQualitativeStandartRepository
{
    SystemdatabaseContext _context;
    public QualitativeStandartRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<QualitativeStandart> GetAllQualitativeStandarts()
    {
        return _context.QualitativeStandarts.ToList();
    }

    public QualitativeStandart GetQualitativeStandartByQualtityStandartId(long qualtityStandartId)
    {
        return _context.QualitativeStandarts.First(x => x.QualtityStandartId == qualtityStandartId);
    }

    public bool CreateQualitativeStandart(QualitativeStandart qualitativeStandart)
    {
        _context.QualitativeStandarts.Add(qualitativeStandart);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateQualitativeStandart(QualitativeStandart qualitativeStandart)
    {
        var existing = _context.QualitativeStandarts.First(x =>
            x.QualtityStandartId == qualitativeStandart.QualtityStandartId);
        existing.AnalysisWorkId = qualitativeStandart.AnalysisWorkId;
        existing.RefGroupId = qualitativeStandart.RefGroupId;
        _context.SaveChanges();
        return true;
    }

    public bool UpdateQualitativeStandartRefGroup(long qualtityStandartId, int refGroupId)
    {
        var existing = _context.QualitativeStandarts.First(x => x.QualtityStandartId == qualtityStandartId);
        existing.RefGroupId = refGroupId;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteQualitativeStandart(long qualtityStandartId)
    {
        var qualitativeStandart = _context.QualitativeStandarts.First(x => x.QualtityStandartId == qualtityStandartId);
        _context.QualitativeStandarts.Remove(qualitativeStandart);
        _context.SaveChanges();
        return true;
    }
}
