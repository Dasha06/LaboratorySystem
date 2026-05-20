using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IQualitativeStandartRepository
{
    List<QualitativeStandart> GetAllQualitativeStandarts();
    QualitativeStandart GetQualitativeStandartByQualtityStandartId(long qualtityStandartId);
    bool CreateQualitativeStandart(QualitativeStandart qualitativeStandart);
    bool UpdateQualitativeStandart(QualitativeStandart qualitativeStandart);
    bool UpdateQualitativeStandartRefGroup(long qualtityStandartId, int refGroupId);
    bool DeleteQualitativeStandart(long qualtityStandartId);
}
