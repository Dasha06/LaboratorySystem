using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class MeasurementRepositoryImpl : IMeasurementRepository
{
    SystemdatabaseContext _context;
    public MeasurementRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Measurement> GetAllMeasurements()
    {
        return _context.Measurements.ToList();
    }

    public Measurement GetMeasurementByMeasurementId(int measurementId)
    {
        return _context.Measurements.First(x => x.MeasurementId == measurementId);
    }

    public bool CreateMeasurement(Measurement measurement)
    {
        _context.Measurements.Add(measurement);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteMeasurement(int measurementId)
    {
        var measurement = _context.Measurements.First(x => x.MeasurementId == measurementId);
        _context.Measurements.Remove(measurement);
        _context.SaveChanges();
        return true;
    }
}
