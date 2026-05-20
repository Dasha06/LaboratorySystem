using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class LpuRepositoryImpl : ILpuRepository
{
    SystemdatabaseContext _context;
    public LpuRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Lpu> GetAllLpus()
    {
        return _context.Lpus.ToList();
    }

    public Lpu GetLpuByLpuId(long lpuId)
    {
        return _context.Lpus.First(x => x.LpuId == lpuId);
    }

    public List<LpuContract> GetLpuContractsByLpuId(long lpuId)
    {
        return _context.LpuContracts
            .Include(x => x.Contract)
            .Where(x => x.LpuId == lpuId)
            .ToList();
    }

    public List<Doctor> GetDoctorsByLpuId(long lpuId)
    {
        return _context.Doctors.Where(x => x.LpuId == lpuId).ToList();
    }

    public bool CreateLpu(Lpu lpu)
    {
        _context.Lpus.Add(lpu);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateLpu(Lpu lpu)
    {
        var existing = _context.Lpus.First(x => x.LpuId == lpu.LpuId);
        existing.LpuName = lpu.LpuName;
        existing.LpuEmail = lpu.LpuEmail;
        _context.SaveChanges();
        return true;
    }

    public bool SetLpuEmail(long lpuId, string? email)
    {
        var existing = _context.Lpus.First(x => x.LpuId == lpuId);
        existing.LpuEmail = email;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteLpu(long lpuId)
    {
        var lpu = _context.Lpus.First(x => x.LpuId == lpuId);
        _context.Lpus.Remove(lpu);
        _context.SaveChanges();
        return true;
    }
}
