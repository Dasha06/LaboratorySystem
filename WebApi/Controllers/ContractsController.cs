using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class ContractsController : ApiControllerBase
{
    private readonly IContractRepository _repository;

    public ContractsController(IContractRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Contract>> GetAll() => Execute(_repository.GetAllContracts);

    [HttpGet("{contractId:long}")]
    public ActionResult<Contract> GetById(long contractId) =>
        Execute(() => _repository.GetContractByContractId(contractId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] ContractForm form) =>
        Execute(() => _repository.CreateContract(FormEntityMapper.ToContract(form)));

    [HttpPut("{contractId:long}")]
    [FormInput]
    public IActionResult Update(long contractId, [FromForm] ContractForm form)
    {
        var entity = FormEntityMapper.ToContract(form);
        entity.ContractId = contractId;
        return Execute(() => _repository.UpdateContract(entity));
    }

    [HttpPatch("{contractId:long}/money-limit")]
    [FormInput]
    public IActionResult UpdateMoneyLimit(long contractId, [FromForm] UpdateContractMoneyLimitForm form) =>
        Execute(() => _repository.UpdateContractMoneyLimit(contractId, form.ContractMoney, form.ContractRemainsMoney));

    [HttpDelete("{contractId:long}")]
    public IActionResult Delete(long contractId) => Execute(() => _repository.DeleteContract(contractId));
}
