using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IQuantitativeStandartRepository
{
    List<QuantitativeStandart> GetAllQuantitativeStandarts();
    QuantitativeStandart GetQuantitativeStandartByQuantStandartId(int quantStandartId);
    bool CreateQuantitativeStandart(QuantitativeStandart quantitativeStandart);
    bool UpdateQuantitativeStandart(QuantitativeStandart quantitativeStandart);
    bool UpdateQuantitativeStandartRefGroup(int quantStandartId, int refGroupId);
    bool DeleteQuantitativeStandart(int quantStandartId);
}
