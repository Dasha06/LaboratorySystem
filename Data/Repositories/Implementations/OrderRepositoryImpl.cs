using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class OrderRepositoryImpl : IOrderRepository
{
    SystemdatabaseContext _context;
    public OrderRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Order> GetAllOrders()
    {
        return _context.Orders.ToList();
    }

    public Order GetOrderByOrderId(long orderId)
    {
        return _context.Orders.First(x => x.OrderId == orderId);
    }

    public bool CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteOrder(long orderId)
    {
        var order = _context.Orders.First(x => x.OrderId == orderId);
        _context.Orders.Remove(order);
        _context.SaveChanges();
        return true;
    }
}
