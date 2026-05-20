using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IBarcodeMaterialRepository
{
    List<BarcodeMaterial> GetAllBarcodeMaterials();
    BarcodeMaterial GetBarcodeMaterialByBarcodeMatIdAndAnalysisDepId(decimal barcodeMatId, int analysisDepId);
    List<BarcodeMaterial> GetBarcodeMaterialsByOrderId(long orderId);
    bool CreateBarcodeMaterial(BarcodeMaterial barcodeMaterial);
    bool UpdateBarcodeMaterial(BarcodeMaterial barcodeMaterial);
    bool DeleteBarcodeMaterial(decimal barcodeMatId, int analysisDepId);
}
