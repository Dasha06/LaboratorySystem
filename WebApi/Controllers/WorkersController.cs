using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class WorkersController : ApiControllerBase
{
    private readonly IWorkerRepository _repository;

    public WorkersController(IWorkerRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Worker>> GetAll() => Execute(_repository.GetAllWorkers);

    [HttpGet("with-roles")]
    public ActionResult<List<Worker>> GetAllWithRoles() => Execute(_repository.GetAllWorkersWithRoles);

    [HttpGet("login")]
    public ActionResult<Worker> Login(string login, string password) =>
        Execute(() => _repository.GetWorkerByLoginAndPassword(login, password));

    [HttpGet("order-counts")]
    public ActionResult<Array> GetOrderCountsByDateRange(
        [FromQuery] DateOnly startDate,
        [FromQuery] DateOnly endDate) =>
        Execute(() => _repository.GetCountOnCreatedOrdersByWorkersWithDates(startDate, endDate));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] WorkerForm form) =>
        Execute(() => _repository.CreateWorker(FormEntityMapper.ToWorker(form)));

    [HttpPut("{workerId:int}")]
    [FormInput]
    public IActionResult Update(int workerId, [FromForm] WorkerForm form)
    {
        var entity = FormEntityMapper.ToWorker(form);
        entity.WorkerId = workerId;
        return Execute(() => _repository.UpdateWorker(entity));
    }

    [HttpPut("{workerId:int}/roles")]
    [FormInput]
    public IActionResult UpdateRoles(int workerId, [FromForm] UpdateWorkerRolesForm form) =>
        Execute(() => _repository.UpdateWorkersRole(workerId, FormEntityMapper.ParseRoles(form.RoleIds)));

    [HttpDelete("{workerId:int}")]
    public IActionResult Delete(int workerId) => Execute(() => _repository.DeleteWorker(workerId));
}
