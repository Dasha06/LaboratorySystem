using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IBarcodeComplexRepository
{
    List<BarcodeComplex> GetAllBarcodeComplexes();
    List<BarcodeComplex> GetBarcodeComplexesByBarcodeMatIdAndAnalysisDepId(decimal barcodeMatId, int analysisDepId);
    BarcodeComplex GetBarcodeComplexByKeys(decimal barcodeMatId, int complexId);
    bool CreateBarcodeComplex(BarcodeComplex barcodeComplex);
    bool UpdateBarcodeComplex(BarcodeComplex barcodeComplex);
    bool DeleteBarcodeComplex(decimal barcodeMatId, int complexId);
}
