using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IPatientRepository
{
    List<Patient> GetAllPatients();
    Patient GetPatientByPatientId(long patientId);
    bool CreatePatient(Patient patient);
    bool DeletePatient(long patientId);
}
