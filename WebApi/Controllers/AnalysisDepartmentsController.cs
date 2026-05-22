using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class AnalysisDepartmentsController : ApiControllerBase
{
    private readonly IAnalysisDepartmentRepository _repository;

    public AnalysisDepartmentsController(IAnalysisDepartmentRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<AnalysisDepartment>> GetAll() => Execute(_repository.GetAllAnalysisDepartments);

    [HttpGet("{analysisDepId:int}")]
    public ActionResult<AnalysisDepartment> GetById(int analysisDepId) =>
        Execute(() => _repository.GetAnalysisDepartmentByAnalysisDepId(analysisDepId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] AnalysisDepartmentForm form) =>
        Execute(() => _repository.CreateAnalysisDepartment(FormEntityMapper.ToAnalysisDepartment(form)));

    [HttpPut("{analysisDepId:int}")]
    [FormInput]
    public IActionResult Update(int analysisDepId, [FromForm] AnalysisDepartmentForm form)
    {
        var entity = FormEntityMapper.ToAnalysisDepartment(form);
        entity.AnalysisDepId = analysisDepId;
        return Execute(() => _repository.UpdateAnalysisDepartment(entity));
    }

    [HttpDelete("{analysisDepId:int}")]
    public IActionResult Delete(int analysisDepId) =>
        Execute(() => _repository.DeleteAnalysisDepartment(analysisDepId));
}
