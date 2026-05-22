using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class QualityParametersController : ApiControllerBase
{
    private readonly IQualityParameterRepository _repository;

    public QualityParametersController(IQualityParameterRepository repository) => _repository = repository;

    [HttpGet("by-standart/{qualitativeStandartId:long}")]
    public ActionResult<List<QualityParameter>> GetByStandart(long qualitativeStandartId) =>
        Execute(() => _repository.GetQualityParametersByQualitativeStandartId(qualitativeStandartId));

    [HttpGet("{qualityParamId:long}/{qualitativeStandartId:long}")]
    public ActionResult<QualityParameter> GetByKeys(long qualityParamId, long qualitativeStandartId)
    {
        var item = _repository.GetQualityParameterByKeys(qualityParamId, qualitativeStandartId);
        return item == null ? NotFound() : item;
    }

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] QualityParameterForm form) =>
        Execute(() => _repository.CreateQualityParameter(FormEntityMapper.ToQualityParameter(form)));

    [HttpPut("{qualityParamId:long}/{qualitativeStandartId:long}")]
    [FormInput]
    public IActionResult Update(long qualityParamId, long qualitativeStandartId, [FromForm] QualityParameterForm form)
    {
        var entity = FormEntityMapper.ToQualityParameter(form);
        entity.QualityParamId = qualityParamId;
        entity.QualitativeStandartId = qualitativeStandartId;
        return Execute(() => _repository.UpdateQualityParameter(entity));
    }

    [HttpDelete("{qualityParamId:long}/{qualitativeStandartId:long}")]
    public IActionResult Delete(long qualityParamId, long qualitativeStandartId) =>
        Execute(() => _repository.DeleteQualityParameter(qualityParamId, qualitativeStandartId));
}
