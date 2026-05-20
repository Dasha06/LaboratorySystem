using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class QualityParameterRepositoryImpl : IQualityParameterRepository
{
    private readonly SystemdatabaseContext _context;

    public QualityParameterRepositoryImpl(SystemdatabaseContext context)
    {
        _context = context;
    }

    public List<QualityParameter> GetQualityParametersByQualitativeStandartId(long qualitativeStandartId)
    {
        return _context.QualityParameters
            .Where(q => q.QualitativeStandartId == qualitativeStandartId)
            .ToList();
    }

    public QualityParameter? GetQualityParameterByKeys(long qualityParamId, long qualitativeStandartId)
    {
        return _context.QualityParameters.FirstOrDefault(q =>
            q.QualityParamId == qualityParamId && q.QualitativeStandartId == qualitativeStandartId);
    }

    public bool CreateQualityParameter(QualityParameter qualityParameter)
    {
        _context.QualityParameters.Add(qualityParameter);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateQualityParameter(QualityParameter qualityParameter)
    {
        var existing = _context.QualityParameters.First(q =>
            q.QualityParamId == qualityParameter.QualityParamId &&
            q.QualitativeStandartId == qualityParameter.QualitativeStandartId);
        existing.QualityCondition = qualityParameter.QualityCondition;
        existing.QualityDescription = qualityParameter.QualityDescription;
        existing.QualityTypeCondition = qualityParameter.QualityTypeCondition;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteQualityParameter(long qualityParamId, long qualitativeStandartId)
    {
        var entity = _context.QualityParameters.First(q =>
            q.QualityParamId == qualityParamId && q.QualitativeStandartId == qualitativeStandartId);
        _context.QualityParameters.Remove(entity);
        _context.SaveChanges();
        return true;
    }
}
