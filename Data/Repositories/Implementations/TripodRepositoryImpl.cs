using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class TripodRepositoryImpl : ITripodRepository
{
    SystemdatabaseContext _context;
    public TripodRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    { 
        _context = remoteDatabaseContext;
    }

    public List<Tripod> GetAllTripods()
    {
        return _context.Tripods.Include(x => x.AnalysisDepartment).ToList();
    }

    public Tripod GetTripodByTripodId(long tripodId)
    {
        return _context.Tripods
            .Include(x => x.AnalysisDepartment)
            .First(x => x.TripodId == tripodId);
    }

    public bool CreateTripod(Tripod tripod)
    {
        _context.Tripods.Add(tripod);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateTripod(Tripod tripod)
    {
        var existing = _context.Tripods.First(x => x.TripodId == tripod.TripodId);
        existing.TripodName = tripod.TripodName;
        existing.TripodCreateDate = tripod.TripodCreateDate;
        existing.TripodMaxCell = tripod.TripodMaxCell;
        existing.AnalysisDepartmentId = tripod.AnalysisDepartmentId;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteTripod(long tripodId)
    {
        var tripod = _context.Tripods.First(x => x.TripodId == tripodId);
        _context.Tripods.Remove(tripod);
        _context.SaveChanges();
        return true;
    }
}
