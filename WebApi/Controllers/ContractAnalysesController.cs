using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class ContractAnalysesController : ApiControllerBase
{
    private readonly IContractAnalysiseRepository _repository;

    public ContractAnalysesController(IContractAnalysiseRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<ContractAnalysise>> GetAll() => Execute(_repository.GetAllContractAnalysises);

    [HttpGet("by-contract/{contractId:long}")]
    public ActionResult<List<ContractAnalysise>> GetByContract(long contractId) =>
        Execute(() => _repository.GetContractAnalysisesByContractId(contractId));

    [HttpGet("by-contract/{contractId:long}/available-analyses")]
    public ActionResult<List<Analysise>> GetAvailableAnalyses(long contractId) =>
        Execute(() => _repository.GetAnalysesAvailableForContract(contractId));

    [HttpGet("{contractId:long}/{analysisId:long}")]
    public ActionResult<ContractAnalysise> GetByKeys(long contractId, long analysisId) =>
        Execute(() => _repository.GetContractAnalysiseByContractIdAndAnalysisId(contractId, analysisId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] ContractAnalysiseForm form) =>
        Execute(() => _repository.CreateContractAnalysise(FormEntityMapper.ToContractAnalysise(form)));

    [HttpPut("{contractId:long}/{analysisId:long}")]
    [FormInput]
    public IActionResult Update(long contractId, long analysisId, [FromForm] ContractAnalysiseForm form)
    {
        var entity = FormEntityMapper.ToContractAnalysise(form);
        entity.ContractId = contractId;
        entity.AnalysisId = analysisId;
        return Execute(() => _repository.UpdateContractAnalysise(entity));
    }

    [HttpDelete("{contractId:long}/{analysisId:long}")]
    public IActionResult Delete(long contractId, long analysisId) =>
        Execute(() => _repository.DeleteContractAnalysise(contractId, analysisId));
}
