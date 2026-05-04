using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class OrderChangeRepositoryImpl : IOrderChangeRepository
{
    SystemdatabaseContext _context;
    public OrderChangeRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<OrderChange> GetAllOrderChanges()
    {
        return _context.OrderChanges.ToList();
    }

    public OrderChange GetOrderChangeByOrderIdAndWorkerIdAndOrderChangeTime(long orderId, int workerId,
        DateTime orderChangeTime)
    {
        return _context.OrderChanges.First(x =>
            x.OrderId == orderId && x.WorkerId == workerId && x.OrderChangeTime == orderChangeTime);
    }

    public bool CreateOrderChange(OrderChange orderChange)
    {
        _context.OrderChanges.Add(orderChange);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteOrderChange(long orderId, int workerId, DateTime orderChangeTime)
    {
        var orderChange = _context.OrderChanges.First(x =>
            x.OrderId == orderId && x.WorkerId == workerId && x.OrderChangeTime == orderChangeTime);
        _context.OrderChanges.Remove(orderChange);
        _context.SaveChanges();
        return true;
    }
}
