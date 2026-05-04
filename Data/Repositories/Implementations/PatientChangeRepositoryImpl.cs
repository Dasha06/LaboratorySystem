using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class PatientChangeRepositoryImpl : IPatientChangeRepository
{
    SystemdatabaseContext _context;
    public PatientChangeRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<PatientChange> GetAllPatientChanges()
    {
        return _context.PatientChanges.ToList();
    }

    public PatientChange GetPatientChangeByPatientIdAndWorkerIdAndPatientChangeTime(long patientId, int workerId,
        DateTime patientChangeTime)
    {
        return _context.PatientChanges.First(x =>
            x.PatientId == patientId && x.WorkerId == workerId && x.PatientChangeTime == patientChangeTime);
    }

    public bool CreatePatientChange(PatientChange patientChange)
    {
        _context.PatientChanges.Add(patientChange);
        _context.SaveChanges();
        return true;
    }

    public bool DeletePatientChange(long patientId, int workerId, DateTime patientChangeTime)
    {
        var patientChange = _context.PatientChanges.First(x =>
            x.PatientId == patientId && x.WorkerId == workerId && x.PatientChangeTime == patientChangeTime);
        _context.PatientChanges.Remove(patientChange);
        _context.SaveChanges();
        return true;
    }
}
