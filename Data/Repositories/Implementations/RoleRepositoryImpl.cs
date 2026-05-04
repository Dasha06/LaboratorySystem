using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class RoleRepositoryImpl : IRoleRepository
{
    SystemdatabaseContext _context;
    public RoleRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Role> GetAllRoles()
    {
        return _context.Roles.ToList();
    }

    public Role GetRoleByRoleId(int roleId)
    {
        return _context.Roles.First(x => x.RoleId == roleId);
    }

    public bool CreateRole(Role role)
    {
        _context.Roles.Add(role);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteRole(int roleId)
    {
        var role = _context.Roles.First(x => x.RoleId == roleId);
        _context.Roles.Remove(role);
        _context.SaveChanges();
        return true;
    }
}
