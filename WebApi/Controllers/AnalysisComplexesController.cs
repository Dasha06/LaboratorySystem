using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class AnalysisComplexesController : ApiControllerBase
{
    private readonly IAnalysisComplexRepository _repository;

    public AnalysisComplexesController(IAnalysisComplexRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<AnalysisComplex>> GetAll() => Execute(_repository.GetAllAnalysisComplexes);

    [HttpGet("{complexId:int}")]
    public ActionResult<AnalysisComplex> GetById(int complexId) =>
        Execute(() => _repository.GetAnalysisComplexByComplexId(complexId));

    [HttpGet("{complexId:int}/material-options")]
    public ActionResult<List<AnalysisWork>> GetMaterialOptions(int complexId) =>
        Execute(() => _repository.GetAnalysisWorksMaterialOptionsForComplex(complexId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] AnalysisComplexForm form) =>
        Execute(() => _repository.CreateAnalysisComplex(FormEntityMapper.ToAnalysisComplex(form)));

    [HttpPut("{complexId:int}")]
    [FormInput]
    public IActionResult Update(int complexId, [FromForm] AnalysisComplexForm form)
    {
        var entity = FormEntityMapper.ToAnalysisComplex(form);
        entity.ComplexId = complexId;
        return Execute(() => _repository.UpdateAnalysisComplex(entity));
    }

    [HttpPost("{complexId:int}/analyses/{analysisId:long}")]
    public IActionResult AddAnalysis(int complexId, long analysisId) =>
        Execute(() => _repository.AddAnalysiseToComplex(complexId, analysisId));

    [HttpDelete("{complexId:int}/analyses/{analysisId:long}")]
    public IActionResult RemoveAnalysis(int complexId, long analysisId) =>
        Execute(() => _repository.RemoveAnalysiseFromComplex(complexId, analysisId));

    [HttpDelete("{complexId:int}")]
    public IActionResult Delete(int complexId) => Execute(() => _repository.DeleteAnalysisComplex(complexId));
}
