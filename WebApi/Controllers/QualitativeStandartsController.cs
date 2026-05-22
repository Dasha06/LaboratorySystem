using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class QualitativeStandartsController : ApiControllerBase
{
    private readonly IQualitativeStandartRepository _repository;

    public QualitativeStandartsController(IQualitativeStandartRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<QualitativeStandart>> GetAll() => Execute(_repository.GetAllQualitativeStandarts);

    [HttpGet("{qualtityStandartId:long}")]
    public ActionResult<QualitativeStandart> GetById(long qualtityStandartId) =>
        Execute(() => _repository.GetQualitativeStandartByQualtityStandartId(qualtityStandartId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] QualitativeStandartForm form) =>
        Execute(() => _repository.CreateQualitativeStandart(FormEntityMapper.ToQualitativeStandart(form)));

    [HttpPut("{qualtityStandartId:long}")]
    [FormInput]
    public IActionResult Update(long qualtityStandartId, [FromForm] QualitativeStandartForm form)
    {
        var entity = FormEntityMapper.ToQualitativeStandart(form);
        entity.QualtityStandartId = qualtityStandartId;
        return Execute(() => _repository.UpdateQualitativeStandart(entity));
    }

    [HttpPatch("{qualtityStandartId:long}/ref-group")]
    [FormInput]
    public IActionResult UpdateRefGroup(long qualtityStandartId, [FromForm] UpdateRefGroupForm form) =>
        Execute(() => _repository.UpdateQualitativeStandartRefGroup(qualtityStandartId, form.RefGroupId));

    [HttpDelete("{qualtityStandartId:long}")]
    public IActionResult Delete(long qualtityStandartId) =>
        Execute(() => _repository.DeleteQualitativeStandart(qualtityStandartId));
}
