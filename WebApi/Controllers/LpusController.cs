using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class LpusController : ApiControllerBase
{
    private readonly ILpuRepository _repository;

    public LpusController(ILpuRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Lpu>> GetAll() => Execute(_repository.GetAllLpus);

    [HttpGet("{lpuId:long}")]
    public ActionResult<Lpu> GetById(long lpuId) => Execute(() => _repository.GetLpuByLpuId(lpuId));

    [HttpGet("{lpuId:long}/contracts")]
    public ActionResult<List<LpuContract>> GetContracts(long lpuId) =>
        Execute(() => _repository.GetLpuContractsByLpuId(lpuId));

    [HttpGet("{lpuId:long}/doctors")]
    public ActionResult<List<Doctor>> GetDoctors(long lpuId) =>
        Execute(() => _repository.GetDoctorsByLpuId(lpuId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] LpuForm form) =>
        Execute(() => _repository.CreateLpu(FormEntityMapper.ToLpu(form)));

    [HttpPut("{lpuId:long}")]
    [FormInput]
    public IActionResult Update(long lpuId, [FromForm] LpuForm form)
    {
        var entity = FormEntityMapper.ToLpu(form);
        entity.LpuId = lpuId;
        return Execute(() => _repository.UpdateLpu(entity));
    }

    [HttpPatch("{lpuId:long}/email")]
    [FormInput]
    public IActionResult SetEmail(long lpuId, [FromForm] SetLpuEmailForm form) =>
        Execute(() => _repository.SetLpuEmail(lpuId, form.Email));

    [HttpDelete("{lpuId:long}")]
    public IActionResult Delete(long lpuId) => Execute(() => _repository.DeleteLpu(lpuId));
}
