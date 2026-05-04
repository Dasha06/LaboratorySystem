using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IDoctorRepository
{
    List<Doctor> GetAllDoctors();
    Doctor GetDoctorByDocId(long docId);
    bool CreateDoctor(Doctor doctor);
    bool DeleteDoctor(long docId);
}
