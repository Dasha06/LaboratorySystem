using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IOrderRepository
{
    List<Order> GetAllOrders();
    Order GetOrderByOrderId(long orderId);
    bool CreateOrder(Order order);
    bool DeleteOrder(long orderId);
}
