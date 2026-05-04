using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IOrderChangeRepository
{
    List<OrderChange> GetAllOrderChanges();
    OrderChange GetOrderChangeByOrderIdAndWorkerIdAndOrderChangeTime(long orderId, int workerId, DateTime orderChangeTime);
    bool CreateOrderChange(OrderChange orderChange);
    bool DeleteOrderChange(long orderId, int workerId, DateTime orderChangeTime);
}
