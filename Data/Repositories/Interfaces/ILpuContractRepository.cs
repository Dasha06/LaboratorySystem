using Data.Models;

namespace Data.Repositories.Interfaces;

public interface ILpuContractRepository
{
    List<LpuContract> GetAllLpuContracts();
    LpuContract GetLpuContractByConLpuId(long conLpuId);
    List<LpuContract> GetLpuContractsByLpuId(long lpuId);
    List<LpuContract> GetLpuContractsByContractId(long contractId);
    bool CreateLpuContract(LpuContract lpuContract);
    bool UpdateLpuContract(LpuContract lpuContract);
    bool DeleteLpuContract(long conLpuId);
}
