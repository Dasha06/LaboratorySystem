using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class RolesController : ApiControllerBase
{
    private readonly IRoleRepository _repository;

    public RolesController(IRoleRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Role>> GetAll() => Execute(_repository.GetAllRoles);

    [HttpGet("{roleId:int}")]
    public ActionResult<Role> GetById(int roleId) => Execute(() => _repository.GetRoleByRoleId(roleId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] RoleForm form) =>
        Execute(() => _repository.CreateRole(FormEntityMapper.ToRole(form)));

    [HttpDelete("{roleId:int}")]
    public IActionResult Delete(int roleId) => Execute(() => _repository.DeleteRole(roleId));
}
