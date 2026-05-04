using Data.Models;

namespace Data.Repositories.Interfaces;

public interface ILpuContractRepository
{
    List<LpuContract> GetAllLpuContracts();
    LpuContract GetLpuContractByConLpuId(long conLpuId);
    bool CreateLpuContract(LpuContract lpuContract);
    bool DeleteLpuContract(long conLpuId);
}
