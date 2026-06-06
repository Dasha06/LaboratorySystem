using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class PatientRepositoryImpl : IPatientRepository
{
    SystemdatabaseContext _context;
    public PatientRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Patient> GetAllPatients()
    {
        return _context.Patients.ToList();
    }

    public Patient GetPatientByPatientId(long patientId)
    {
        return _context.Patients.First(x => x.PatientId == patientId);
    }

    public bool CreatePatient(Patient patient)
    {
        _context.Patients.Add(patient);
        _context.SaveChanges();
        return true;
    }

    public bool UpdatePatient(Patient patient)
    {
        var existing = _context.Patients.First(x => x.PatientId == patient.PatientId);
        existing.PatientFirstName = patient.PatientFirstName;
        existing.PatientLastName = patient.PatientLastName;
        existing.PatientSecondName = patient.PatientSecondName;
        existing.PatientBirthday = patient.PatientBirthday;
        existing.PatientGender = patient.PatientGender;
        existing.PatientEmail = patient.PatientEmail;
        _context.SaveChanges();
        return true;
    }

    public bool CreatePatientChange(PatientChange patientChange)
    {
        _context.PatientChanges.Add(patientChange);
        _context.SaveChanges();
        return true;
    }

    public bool DeletePatient(long patientId)
    {
        var patient = _context.Patients.First(x => x.PatientId == patientId);
        _context.Patients.Remove(patient);
        _context.SaveChanges();
        return true;
    }
}
