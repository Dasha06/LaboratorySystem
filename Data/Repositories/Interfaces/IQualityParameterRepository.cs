using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IQualityParameterRepository
{
    List<QualityParameter> GetQualityParametersByQualitativeStandartId(long qualitativeStandartId);
    QualityParameter? GetQualityParameterByKeys(long qualityParamId, long qualitativeStandartId);
    bool CreateQualityParameter(QualityParameter qualityParameter);
    bool UpdateQualityParameter(QualityParameter qualityParameter);
    bool DeleteQualityParameter(long qualityParamId, long qualitativeStandartId);
}
