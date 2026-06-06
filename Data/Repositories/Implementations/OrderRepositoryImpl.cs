using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        return _context.Orders
            .Include(o => o.Lpu)
            .Include(o => o.Patient)
            .Include(o => o.OrderChanges)
            .ToList();
    }

    public Order GetOrderByOrderId(long orderId)
    {
        return _context.Orders
            .Include(o => o.Lpu)
            .Include(o => o.Patient)
            .Include(o => o.OrderChanges)
            .Include(o => o.OrderChanges)
            .First(x => x.OrderId == orderId);
    }

    public Order GetOrderWithAnalysesAndBarcodes(long orderId)
    {
        return _context.Orders
            .Include(o => o.Patient)
            .Include(o => o.Lpu)
            .Include(o => o.Doc)
            .Include(o => o.OrderChanges)
            .Include(o => o.BarcodeMaterials)
            .ThenInclude(bm => bm.Material)
            .Include(o => o.BarcodeMaterials)
            .ThenInclude(bm => bm.AnalysisDep)
            .Include(o => o.BarcodeMaterials)
            .ThenInclude(bm => bm.BarcodeAnalysises)
            .ThenInclude(ba => ba.Analysis)
            .Include(o => o.BarcodeMaterials)
            .ThenInclude(bm => bm.BarcodeComplexes)
            .ThenInclude(bc => bc.Complex)
            .Include(o => o.ConLpus)
            .ThenInclude(c => c.Contract)
            .First(o => o.OrderId == orderId);
    }

    public bool CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateOrder(Order order)
    {
        var existing = _context.Orders.First(x => x.OrderId == order.OrderId);
        existing.DocId = order.DocId;
        existing.OrderLpuDepartment = order.OrderLpuDepartment;
        existing.OrderStatus = order.OrderStatus;
        existing.PatientId = order.PatientId;
        existing.LpuId = order.LpuId;
        existing.OrderIsCountingInContract = order.OrderIsCountingInContract;
        existing.OrderTakenDate = order.OrderTakenDate;
        _context.SaveChanges();
        return true;
    }

    public bool CreateOrderChange(OrderChange orderChange)
    {
        _context.OrderChanges.Add(orderChange);
        _context.SaveChanges();
        return true;
    }

    public bool ApplyOrderCostToLinkedContracts(long orderId)
    {
        var order = _context.Orders
            .Include(o => o.ConLpus)
            .Include(o => o.BarcodeMaterials)
            .ThenInclude(bm => bm.BarcodeAnalysises)
            .Include(o => o.BarcodeMaterials)
            .ThenInclude(bm => bm.BarcodeComplexes)
            .First(o => o.OrderId == orderId);

        if (!order.OrderIsCountingInContract || order.ConLpus.Count == 0)
            return true;

        var contractIds = order.ConLpus.Select(c => c.ContractId).Distinct().ToList();

        foreach (var contractId in contractIds)
        {
            var contract = _context.Contracts.First(c => c.ContractId == contractId);
            double total = 0;

            foreach (var bm in order.BarcodeMaterials)
            {
                foreach (var ba in bm.BarcodeAnalysises)
                {
                    var row = _context.ContractAnalysises.FirstOrDefault(x =>
                        x.ContractId == contractId && x.AnalysisId == ba.AnalysisId);
                    if (row != null)
                        total += row.ContrAnalysisCost;
                }

                foreach (var bc in bm.BarcodeComplexes)
                {
                    var row = _context.ContractComplexes.FirstOrDefault(x =>
                        x.ContractId == contractId && x.ComplexId == bc.ComplexId);
                    if (row != null)
                        total += row.ContractComplexCost;
                }
            }

            contract.ContractRemainsMoney -= total;
        }

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
