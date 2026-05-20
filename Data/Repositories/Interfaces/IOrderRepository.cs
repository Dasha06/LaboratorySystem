using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IOrderRepository
{
    List<Order> GetAllOrders();
    Order GetOrderByOrderId(long orderId);
    Order GetOrderWithAnalysesAndBarcodes(long orderId);
    bool CreateOrder(Order order);
    bool UpdateOrder(Order order);
    /// <summary>
    /// Списать стоимость анализов и комплексов по прайсу контракта с остатка <see cref="Contract.ContractRemainsMoney"/>,
    /// если у заказа <see cref="Order.OrderIsCountingInContract"/> и привязаны связи ЛПУ–контракт.
    /// </summary>
    bool ApplyOrderCostToLinkedContracts(long orderId);
    bool DeleteOrder(long orderId);
}
