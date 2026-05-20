using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class DoctorRepositoryImpl : IDoctorRepository
{
    SystemdatabaseContext _context;
    public DoctorRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Doctor> GetAllDoctors()
    {
        return _context.Doctors.ToList();
    }

    public Doctor GetDoctorByDocId(long docId)
    {
        return _context.Doctors.First(x => x.DocId == docId);
    }

    public List<Doctor> GetDoctorsByLpuId(long lpuId)
    {
        return _context.Doctors.Where(x => x.LpuId == lpuId).ToList();
    }

    public bool CreateDoctor(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateDoctor(Doctor doctor)
    {
        var existing = _context.Doctors.First(x => x.DocId == doctor.DocId);
        existing.DocFio = doctor.DocFio;
        existing.LpuId = doctor.LpuId;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteDoctor(long docId)
    {
        var doctor = _context.Doctors.First(x => x.DocId == docId);
        _context.Doctors.Remove(doctor);
        _context.SaveChanges();
        return true;
    }
}
