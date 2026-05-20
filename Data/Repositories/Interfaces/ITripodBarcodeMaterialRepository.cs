using Data.Models;

namespace Data.Repositories.Interfaces;

public interface ITripodBarcodeMaterialRepository
{
    List<TripodBarcodeMaterial> GetAllTripodBarcodeMaterials();
    TripodBarcodeMaterial GetTripodBarcodeMaterialByTripodIdAndBarcodeMatId(long tripodId, decimal barcodeMatId);
    bool CreateTripodBarcodeMaterial(TripodBarcodeMaterial tripodBarcodeMaterial);
    bool UpdateTripodBarcodeMaterial(TripodBarcodeMaterial tripodBarcodeMaterial);
    bool DeleteTripodBarcodeMaterial(long tripodId, decimal barcodeMatId);
}
