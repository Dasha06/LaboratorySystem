using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IQualitativeStandartRepository
{
    List<QualitativeStandart> GetAllQualitativeStandarts();
    QualitativeStandart GetQualitativeStandartByQualtityStandartId(int qualtityStandartId);
    bool CreateQualitativeStandart(QualitativeStandart qualitativeStandart);
    bool DeleteQualitativeStandart(int qualtityStandartId);
}
