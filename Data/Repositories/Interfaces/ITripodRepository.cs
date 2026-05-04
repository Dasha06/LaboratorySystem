using Data.Models;

namespace Data.Repositories.Interfaces;

public interface ITripodRepository
{
    List<Tripod> GetAllTripods();
    Tripod GetTripodByTripodId(long tripodId);
    bool CreateTripod(Tripod tripod);
    bool DeleteTripod(long tripodId);
}
