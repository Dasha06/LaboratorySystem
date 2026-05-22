using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class OrderChangesController : ApiControllerBase
{
    private readonly IOrderChangeRepository _repository;

    public OrderChangesController(IOrderChangeRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<OrderChange>> GetAll() => Execute(_repository.GetAllOrderChanges);

    [HttpGet("{orderId:long}/{workerId:int}")]
    public ActionResult<OrderChange> GetByKeys(long orderId, int workerId, [FromQuery] DateTime orderChangeTime) =>
        Execute(() => _repository.GetOrderChangeByOrderIdAndWorkerIdAndOrderChangeTime(
            orderId, workerId, orderChangeTime));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] OrderChangeForm form) =>
        Execute(() => _repository.CreateOrderChange(FormEntityMapper.ToOrderChange(form)));

    [HttpDelete("{orderId:long}/{workerId:int}")]
    public IActionResult Delete(long orderId, int workerId, [FromQuery] DateTime orderChangeTime) =>
        Execute(() => _repository.DeleteOrderChange(orderId, workerId, orderChangeTime));
}
