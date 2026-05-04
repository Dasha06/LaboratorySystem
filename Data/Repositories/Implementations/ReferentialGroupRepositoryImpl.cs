using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class ReferentialGroupRepositoryImpl : IReferentialGroupRepository
{
    SystemdatabaseContext _context;
    public ReferentialGroupRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<ReferentialGroup> GetAllReferentialGroups()
    {
        return _context.ReferentialGroups.ToList();
    }

    public ReferentialGroup GetReferentialGroupByRefGroupId(int refGroupId)
    {
        return _context.ReferentialGroups.First(x => x.RefGroupId == refGroupId);
    }

    public bool CreateReferentialGroup(ReferentialGroup referentialGroup)
    {
        _context.ReferentialGroups.Add(referentialGroup);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteReferentialGroup(int refGroupId)
    {
        var referentialGroup = _context.ReferentialGroups.First(x => x.RefGroupId == refGroupId);
        _context.ReferentialGroups.Remove(referentialGroup);
        _context.SaveChanges();
        return true;
    }
}
