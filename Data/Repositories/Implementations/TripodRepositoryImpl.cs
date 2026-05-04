using Data.Models;
using Data.Repositories.Interfaces;

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
        return _context.Tripods.ToList();
    }

    public Tripod GetTripodByTripodId(long tripodId)
    {
        return _context.Tripods.First(x => x.TripodId == tripodId);
    }

    public bool CreateTripod(Tripod tripod)
    {
        _context.Tripods.Add(tripod);
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
