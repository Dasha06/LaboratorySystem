using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class LpuContractsController : ApiControllerBase
{
    private readonly ILpuContractRepository _repository;

    public LpuContractsController(ILpuContractRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<LpuContract>> GetAll() => Execute(_repository.GetAllLpuContracts);

    [HttpGet("{conLpuId:long}")]
    public ActionResult<LpuContract> GetById(long conLpuId) =>
        Execute(() => _repository.GetLpuContractByConLpuId(conLpuId));

    [HttpGet("by-lpu/{lpuId:long}")]
    public ActionResult<List<LpuContract>> GetByLpu(long lpuId) =>
        Execute(() => _repository.GetLpuContractsByLpuId(lpuId));

    [HttpGet("by-contract/{contractId:long}")]
    public ActionResult<List<LpuContract>> GetByContract(long contractId) =>
        Execute(() => _repository.GetLpuContractsByContractId(contractId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] LpuContractForm form) =>
        Execute(() => _repository.CreateLpuContract(FormEntityMapper.ToLpuContract(form)));

    [HttpPut("{conLpuId:long}")]
    [FormInput]
    public IActionResult Update(long conLpuId, [FromForm] LpuContractForm form)
    {
        var entity = FormEntityMapper.ToLpuContract(form);
        entity.ConLpuId = conLpuId;
        return Execute(() => _repository.UpdateLpuContract(entity));
    }

    [HttpDelete("{conLpuId:long}")]
    public IActionResult Delete(long conLpuId) => Execute(() => _repository.DeleteLpuContract(conLpuId));
}
