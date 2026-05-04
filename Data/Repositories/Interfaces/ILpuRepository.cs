using Data.Models;

namespace Data.Repositories.Interfaces;

public interface ILpuRepository
{
    List<Lpu> GetAllLpus();
    Lpu GetLpuByLpuId(long lpuId);
    bool CreateLpu(Lpu lpu);
    bool DeleteLpu(long lpuId);
}
