using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IPatientChangeRepository
{
    List<PatientChange> GetAllPatientChanges();
    PatientChange GetPatientChangeByPatientIdAndWorkerIdAndPatientChangeTime(long patientId, int workerId,
        DateTime patientChangeTime);
    bool CreatePatientChange(PatientChange patientChange);
    bool DeletePatientChange(long patientId, int workerId, DateTime patientChangeTime);
}
