using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class QuantitativeStandartsController : ApiControllerBase
{
    private readonly IQuantitativeStandartRepository _repository;

    public QuantitativeStandartsController(IQuantitativeStandartRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<QuantitativeStandart>> GetAll() => Execute(_repository.GetAllQuantitativeStandarts);

    [HttpGet("{quantStandartId:int}")]
    public ActionResult<QuantitativeStandart> GetById(int quantStandartId) =>
        Execute(() => _repository.GetQuantitativeStandartByQuantStandartId(quantStandartId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] QuantitativeStandartForm form) =>
        Execute(() => _repository.CreateQuantitativeStandart(FormEntityMapper.ToQuantitativeStandart(form)));

    [HttpPut("{quantStandartId:int}")]
    [FormInput]
    public IActionResult Update(int quantStandartId, [FromForm] QuantitativeStandartForm form)
    {
        var entity = FormEntityMapper.ToQuantitativeStandart(form);
        entity.QuantStandartId = quantStandartId;
        return Execute(() => _repository.UpdateQuantitativeStandart(entity));
    }

    [HttpPatch("{quantStandartId:int}/ref-group")]
    [FormInput]
    public IActionResult UpdateRefGroup(int quantStandartId, [FromForm] UpdateRefGroupForm form) =>
        Execute(() => _repository.UpdateQuantitativeStandartRefGroup(quantStandartId, form.RefGroupId));

    [HttpDelete("{quantStandartId:int}")]
    public IActionResult Delete(int quantStandartId) =>
        Execute(() => _repository.DeleteQuantitativeStandart(quantStandartId));
}
