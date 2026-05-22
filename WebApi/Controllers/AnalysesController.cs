using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class AnalysesController : ApiControllerBase
{
    private readonly IAnalysiseRepository _repository;
    private readonly IAnalysisDepartmentRepository _departmentRepository;

    public AnalysesController(
        IAnalysiseRepository repository,
        IAnalysisDepartmentRepository departmentRepository)
    {
        _repository = repository;
        _departmentRepository = departmentRepository;
    }

    [HttpGet]
    public ActionResult<List<Analysise>> GetAll() => Execute(_repository.GetAllAnalyses);

    [HttpGet("by-departments")]
    public ActionResult<Array> GetByDepartments() => Execute(_repository.GetAnalysesByDepartments);

    [HttpGet("{analysisId:long}")]
    public ActionResult<Analysise> GetById(long analysisId) =>
        Execute(() => _repository.GetAnalysisByAnalysisId(analysisId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] AnalysiseForm form) =>
        Execute(() => _repository.CreateAnalysis(FormEntityMapper.ToAnalysise(form)));

    [HttpPut("{analysisId:long}")]
    [FormInput]
    public IActionResult Update(long analysisId, [FromForm] AnalysiseForm form)
    {
        var entity = FormEntityMapper.ToAnalysise(form);
        entity.AnalysisId = analysisId;
        return Execute(() => _repository.UpdateAnalysis(entity));
    }

    [HttpPatch("{analysisId:long}/department")]
    [FormInput]
    public IActionResult UpdateDepartment(long analysisId, [FromForm] UpdateAnalysisDepartmentForm form)
    {
        var analysis = _repository.GetAnalysisByAnalysisId(analysisId);
        var department = _departmentRepository.GetAnalysisDepartmentByAnalysisDepId(form.AnalysisDepId);
        return Execute(() => _repository.UpdateDepartmentOfAnalysis(analysis, department));
    }

    [HttpDelete("{analysisId:long}")]
    public IActionResult Delete(long analysisId) => Execute(() => _repository.DeleteAnalysis(analysisId));
}
