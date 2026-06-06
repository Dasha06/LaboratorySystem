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
    public IActionResult Create([FromForm] OrderForm form)
    {
        var order = FormEntityMapper.ToOrder(form);
        var result = _repository.CreateOrder(order);
        if (result && form.WorkerId.HasValue)
        {
            _repository.CreateOrderChange(new OrderChange
            {
                OrderId = order.OrderId,
                WorkerId = form.WorkerId.Value,
                OrderChangeTime = DateTime.Now,
                TypeId = 1 // 1 = создание
            });
        }
        return Execute(() => result);
    }

    [HttpPut("{orderId:long}")]
    [FormInput]
    public IActionResult Update(long orderId, [FromForm] OrderForm form)
    {
        var entity = FormEntityMapper.ToOrder(form);
        entity.OrderId = orderId;
        var result = _repository.UpdateOrder(entity);
        if (result && form.WorkerId.HasValue)
        {
            _repository.CreateOrderChange(new OrderChange
            {
                OrderId = orderId,
                WorkerId = form.WorkerId.Value,
                OrderChangeTime = DateTime.Now,
                TypeId = 2 // 2 = изменение
            });
        }
        return Execute(() => result);
    }

    [HttpPost("{orderId:long}/apply-contract-cost")]
    public IActionResult ApplyContractCost(long orderId) =>
        Execute(() => _repository.ApplyOrderCostToLinkedContracts(orderId));

    [HttpDelete("{orderId:long}")]
    public IActionResult Delete(long orderId) => Execute(() => _repository.DeleteOrder(orderId));
}
