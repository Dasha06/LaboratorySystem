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

    public bool CreateDoctor(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
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
