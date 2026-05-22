using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class ReferentialGroupsController : ApiControllerBase
{
    private readonly IReferentialGroupRepository _repository;

    public ReferentialGroupsController(IReferentialGroupRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<ReferentialGroup>> GetAll() => Execute(_repository.GetAllReferentialGroups);

    [HttpGet("{refGroupId:int}")]
    public ActionResult<ReferentialGroup> GetById(int refGroupId) =>
        Execute(() => _repository.GetReferentialGroupByRefGroupId(refGroupId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] ReferentialGroupForm form) =>
        Execute(() => _repository.CreateReferentialGroup(FormEntityMapper.ToReferentialGroup(form)));

    [HttpPut("{refGroupId:int}")]
    [FormInput]
    public IActionResult Update(int refGroupId, [FromForm] ReferentialGroupForm form)
    {
        var entity = FormEntityMapper.ToReferentialGroup(form);
        entity.RefGroupId = refGroupId;
        return Execute(() => _repository.UpdateReferentialGroup(entity));
    }

    [HttpDelete("{refGroupId:int}")]
    public IActionResult Delete(int refGroupId) => Execute(() => _repository.DeleteReferentialGroup(refGroupId));
}
