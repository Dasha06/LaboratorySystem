using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class AnalysesTemplatesController : ApiControllerBase
{
    private readonly IAnalysisesTemplateRepository _repository;
    private readonly IAnalysiseRepository _analysiseRepository;

    public AnalysesTemplatesController(
        IAnalysisesTemplateRepository repository,
        IAnalysiseRepository analysiseRepository)
    {
        _repository = repository;
        _analysiseRepository = analysiseRepository;
    }

    [HttpGet]
    public ActionResult<List<AnalysesTemplate>> GetAll() => Execute(_repository.GetAllAnalysisesTemplates);

    [HttpGet("{analysisTempId:int}/analyses")]
    public ActionResult<List<Analysise>> GetAnalyses(int analysisTempId) =>
        Execute(() => _repository.GetAnalysesFromTemplate(analysisTempId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] AnalysesTemplateForm form) =>
        Execute(() => _repository.CreateAnalysisesTemplate(BuildTemplate(form)));

    [HttpPut("{analysisTempId:int}")]
    [FormInput]
    public IActionResult Update(int analysisTempId, [FromForm] AnalysesTemplateForm form)
    {
        var template = BuildTemplate(form);
        template.AnalysisTempId = analysisTempId;
        return Execute(() => _repository.UpdateAnalysisTemplate(template));
    }

    [HttpDelete("{analysisTempId:int}")]
    public IActionResult Delete(int analysisTempId) =>
        Execute(() => _repository.DeleteAnalysisesTemplate(analysisTempId));

    private AnalysesTemplate BuildTemplate(AnalysesTemplateForm form)
    {
        var template = new AnalysesTemplate { AnalysisTempName = form.AnalysisTempName };
        foreach (var id in FormEntityMapper.ParseIds(form.AnalysisIds))
            template.Analyses.Add(_analysiseRepository.GetAnalysisByAnalysisId(id));
        return template;
    }
}
