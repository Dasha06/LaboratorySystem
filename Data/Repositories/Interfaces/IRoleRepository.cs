using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IRoleRepository
{
    List<Role> GetAllRoles();
    Role GetRoleByRoleId(int roleId);
    bool CreateRole(Role role);
    bool UpdateRole(Role role);
    bool DeleteRole(int roleId);
}
