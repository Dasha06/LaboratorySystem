using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class AnalysisWorksController : ApiControllerBase
{
    private readonly IAnalysisWorkRepository _repository;
    private readonly IAnalysiseRepository _analysiseRepository;

    public AnalysisWorksController(
        IAnalysisWorkRepository repository,
        IAnalysiseRepository analysiseRepository)
    {
        _repository = repository;
        _analysiseRepository = analysiseRepository;
    }

    [HttpGet]
    public ActionResult<List<AnalysisWork>> GetAll() => Execute(_repository.GetAllAnalysisWorks);

    [HttpGet("{analysisWorkId:long}")]
    public ActionResult<AnalysisWork> GetById(long analysisWorkId) =>
        Execute(() => _repository.GetAnalysisWorkByAnalysisWorkId(analysisWorkId));

    [HttpGet("by-analysis/{analysisId:long}")]
    public ActionResult<List<AnalysisWork>> GetByAnalysis(long analysisId)
    {
        var analysis = _analysiseRepository.GetAnalysisByAnalysisId(analysisId);
        return Execute(() => _repository.GetAnalysisWorkByAnalysis(analysis));
    }

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] AnalysisWorkForm form) =>
        Execute(() => _repository.CreateAnalysisWork(FormEntityMapper.ToAnalysisWork(form)));

    [HttpPut("{analysisWorkId:long}")]
    [FormInput]
    public IActionResult Update(long analysisWorkId, [FromForm] AnalysisWorkForm form)
    {
        var entity = FormEntityMapper.ToAnalysisWork(form);
        entity.AnalysisWorkId = analysisWorkId;
        return Execute(() => _repository.UpdateAnalysisWork(entity));
    }

    [HttpDelete("{analysisWorkId:long}")]
    public IActionResult Delete(long analysisWorkId) =>
        Execute(() => _repository.DeleteAnalysisWork(analysisWorkId));
}
