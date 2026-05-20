using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IDoctorRepository
{
    List<Doctor> GetAllDoctors();
    Doctor GetDoctorByDocId(long docId);
    List<Doctor> GetDoctorsByLpuId(long lpuId);
    bool CreateDoctor(Doctor doctor);
    bool UpdateDoctor(Doctor doctor);
    bool DeleteDoctor(long docId);
}
