using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class TypeChangesController : ApiControllerBase
{
    private readonly ITypeChangeRepository _repository;

    public TypeChangesController(ITypeChangeRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<TypeChange>> GetAll() => Execute(_repository.GetAllTypeChanges);

    [HttpGet("{typeId:int}")]
    public ActionResult<TypeChange> GetById(int typeId) => Execute(() => _repository.GetTypeChangeByTypeId(typeId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] TypeChangeForm form) =>
        Execute(() => _repository.CreateTypeChange(FormEntityMapper.ToTypeChange(form)));

    [HttpDelete("{typeId:int}")]
    public IActionResult Delete(int typeId) => Execute(() => _repository.DeleteTypeChange(typeId));
}
