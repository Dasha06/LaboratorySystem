using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IWorkerRepository
{
    List<Worker> GetAllWorkers();
    List<Worker> GetAllWorkersWithRoles();
    Worker GetWorkerByLoginAndPassword(string login, string password);
    bool CreateWorker(Worker worker);
    bool UpdateWorkersRole(int workerId, List<Role> roles);
    bool DeleteWorker(int workerId);
    Array GetCountOnCreatedOrdersByWorkersWithDates(DateOnly startDate, DateOnly endDate);
    bool UpdateWorker(Worker worker);
}
