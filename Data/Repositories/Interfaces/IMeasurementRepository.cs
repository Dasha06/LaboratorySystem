using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IMeasurementRepository
{
    List<Measurement> GetAllMeasurements();
    Measurement GetMeasurementByMeasurementId(int measurementId);
    bool CreateMeasurement(Measurement measurement);
    bool DeleteMeasurement(int measurementId);
}
