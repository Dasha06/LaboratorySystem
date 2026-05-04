using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IBarcodeMaterialRepository
{
    List<BarcodeMaterial> GetAllBarcodeMaterials();
    BarcodeMaterial GetBarcodeMaterialByBarcodeMatIdAndAnalysisDepId(decimal barcodeMatId, int analysisDepId);
    bool CreateBarcodeMaterial(BarcodeMaterial barcodeMaterial);
    bool DeleteBarcodeMaterial(decimal barcodeMatId, int analysisDepId);
}
