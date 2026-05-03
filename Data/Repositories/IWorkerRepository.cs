using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IWorkerRepository
{
    List<Worker> GetAllWorkers();
    List<Worker> GetAllWorkersWithRoles();
    Worker GetWorkerByLoginAndPassword(string login, string password);
    bool CreateWorker(string name, string login, string password, List<Role> roles);
    bool UpdateWorkersRole(int workerId, List<Role> roles);
    bool DeleteWorker(int workerId);
    Array GetCountOnCreatedOrdersByWorkersWithDates(DateOnly startDate, DateOnly endDate);
    bool CreateWorkers(List<(string name, string login, string password, List<Role> roles)> workersInfo);
    bool UpdateMultipleWorkersRoles(List<(int workerId, List<Role> roles)> workersRoles);
}
