using Data.Models;

namespace Data.Repositories.Interfaces;

public interface ILpuRepository
{
    List<Lpu> GetAllLpus();
    Lpu GetLpuByLpuId(long lpuId);
    List<LpuContract> GetLpuContractsByLpuId(long lpuId);
    List<Doctor> GetDoctorsByLpuId(long lpuId);
    bool CreateLpu(Lpu lpu);
    bool UpdateLpu(Lpu lpu);
    /// <summary>Установить или изменить основной e-mail ЛПУ.</summary>
    bool SetLpuEmail(long lpuId, string? email);
    bool DeleteLpu(long lpuId);
}
