using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class WorkerRepositoryImpl : IWorkerRepository
{
    SystemdatabaseContext _context;
    public WorkerRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Worker> GetAllWorkers()
    {
        return _context.Workers.ToList();
    }

    public List<Worker> GetAllWorkersWithRoles()
    {
        return _context.Workers.Include(x => x.Roles).ToList();
    }

    public Worker GetWorkerByLoginAndPassword(string login, string password)
    {
        return _context.Workers.First(x => x.WorkerLogin == login && x.WorkerPassword == password);
    }

    public bool CreateWorker(string name, string login, string password, List<Role> roles)
    {
        var newWorker = new Worker
        {
            WorkerFio = name,
            WorkerLogin = login,
            WorkerPassword = password,
            Roles = roles
        };
        _context.Workers.Add(newWorker);
        _context.SaveChanges();
        return true;
    }

    public bool CreateWorkers(List<(string name, string login, string password, List<Role> roles)> workersInfo)
    {
        foreach (var workerInfo in workersInfo)
        {
            var newWorker = new Worker
            {
                WorkerFio = workerInfo.name,
                WorkerLogin = workerInfo.login,
                WorkerPassword = workerInfo.password,
                Roles = workerInfo.roles
            };
            _context.Workers.Add(newWorker);
        }

        _context.SaveChanges();
        return true;
    }

    public bool UpdateMultipleWorkersRoles(List<(int workerId, List<Role> roles)> workersRoles)
    {
        foreach (var item in workersRoles)
        {
            var user = _context.Workers
                .Include(w => w.Roles)
                .First(w => w.WorkerId == item.workerId);
            user.Roles = item.roles;
        }

        _context.SaveChanges();
        return true;
    }

    public bool UpdateWorkersRole(int workerId, List<Role> roles)
    {
        var user = _context.Workers.Include(x => x.Roles)
            .First(x => x.WorkerId == workerId);
        user.Roles = roles;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteWorker(int workerId)
    {
        var worker = _context.Workers.First(x => x.WorkerId == workerId);
        _context.Workers.Remove(worker);
        _context.SaveChanges();
        return true;
    }

    public Array GetCountOnCreatedOrdersByWorkersWithDates(DateOnly startDate, DateOnly endDate)
    {
        var ordersCreated = _context.OrderChanges.Where(x => x.Type.TypeName == "Создано"
                                                            && x.OrderChangeTime >= startDate.ToDateTime(TimeOnly.MinValue)
                                                            && x.OrderChangeTime <= endDate.ToDateTime(TimeOnly.MaxValue))
            .GroupBy(x => x.Worker).Select(g => new { Worker = g.Key, Count = g.Count() }).ToArray();
        return ordersCreated;
    }
}
