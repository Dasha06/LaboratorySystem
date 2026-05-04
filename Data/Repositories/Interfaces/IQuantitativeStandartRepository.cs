using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IQuantitativeStandartRepository
{
    List<QuantitativeStandart> GetAllQuantitativeStandarts();
    QuantitativeStandart GetQuantitativeStandartByQuantStandartId(int quantStandartId);
    bool CreateQuantitativeStandart(QuantitativeStandart quantitativeStandart);
    bool DeleteQuantitativeStandart(int quantStandartId);
}
