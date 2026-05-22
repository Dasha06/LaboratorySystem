using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class ContractComplexesController : ApiControllerBase
{
    private readonly IContractComplexRepository _repository;

    public ContractComplexesController(IContractComplexRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<ContractComplex>> GetAll() => Execute(_repository.GetAllContractComplexes);

    [HttpGet("by-contract/{contractId:long}")]
    public ActionResult<List<ContractComplex>> GetByContract(long contractId) =>
        Execute(() => _repository.GetContractComplexesByContractId(contractId));

    [HttpGet("{contractId:long}/{complexId:int}")]
    public ActionResult<ContractComplex> GetByKeys(long contractId, int complexId) =>
        Execute(() => _repository.GetContractComplexByContractIdAndComplexId(contractId, complexId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] ContractComplexForm form) =>
        Execute(() => _repository.CreateContractComplex(FormEntityMapper.ToContractComplex(form)));

    [HttpPut("{contractId:long}/{complexId:int}")]
    [FormInput]
    public IActionResult Update(long contractId, int complexId, [FromForm] ContractComplexForm form)
    {
        var entity = FormEntityMapper.ToContractComplex(form);
        entity.ContractId = contractId;
        entity.ComplexId = complexId;
        return Execute(() => _repository.UpdateContractComplex(entity));
    }

    [HttpDelete("{contractId:long}/{complexId:int}")]
    public IActionResult Delete(long contractId, int complexId) =>
        Execute(() => _repository.DeleteContractComplex(contractId, complexId));
}
