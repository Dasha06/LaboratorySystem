using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class TypeChangeRepositoryImpl : ITypeChangeRepository
{
    SystemdatabaseContext _context;
    public TypeChangeRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<TypeChange> GetAllTypeChanges()
    {
        return _context.TypeChanges.ToList();
    }

    public TypeChange GetTypeChangeByTypeId(int typeId)
    {
        return _context.TypeChanges.First(x => x.TypeId == typeId);
    }

    public bool CreateTypeChange(TypeChange typeChange)
    {
        _context.TypeChanges.Add(typeChange);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteTypeChange(int typeId)
    {
        var typeChange = _context.TypeChanges.First(x => x.TypeId == typeId);
        _context.TypeChanges.Remove(typeChange);
        _context.SaveChanges();
        return true;
    }
}
