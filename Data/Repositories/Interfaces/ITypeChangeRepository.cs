using Data.Models;

namespace Data.Repositories.Interfaces;

public interface ITypeChangeRepository
{
    List<TypeChange> GetAllTypeChanges();
    TypeChange GetTypeChangeByTypeId(int typeId);
    bool CreateTypeChange(TypeChange typeChange);
    bool DeleteTypeChange(int typeId);
}
