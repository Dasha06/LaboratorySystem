using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class OrdersController : ApiControllerBase
{
    private readonly IOrderRepository _repository;

    public OrdersController(IOrderRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Order>> GetAll() => Execute(_repository.GetAllOrders);

    [HttpGet("{orderId:long}")]
    public ActionResult<Order> GetById(long orderId) => Execute(() => _repository.GetOrderByOrderId(orderId));

    [HttpGet("{orderId:long}/details")]
    public ActionResult<Order> GetWithAnalysesAndBarcodes(long orderId) =>
        Execute(() => _repository.GetOrderWithAnalysesAndBarcodes(orderId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] OrderForm form) =>
        Execute(() => _repository.CreateOrder(FormEntityMapper.ToOrder(form)));

    [HttpPut("{orderId:long}")]
    [FormInput]
    public IActionResult Update(long orderId, [FromForm] OrderForm form)
    {
        var entity = FormEntityMapper.ToOrder(form);
        entity.OrderId = orderId;
        return Execute(() => _repository.UpdateOrder(entity));
    }

    [HttpPost("{orderId:long}/apply-contract-cost")]
    public IActionResult ApplyContractCost(long orderId) =>
        Execute(() => _repository.ApplyOrderCostToLinkedContracts(orderId));

    [HttpDelete("{orderId:long}")]
    public IActionResult Delete(long orderId) => Execute(() => _repository.DeleteOrder(orderId));
}
