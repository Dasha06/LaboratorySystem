using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IBarcodeAnalysiseRepository
{
    List<BarcodeAnalysise> GetAllBarcodeAnalysises();
    BarcodeAnalysise GetBarcodeAnalysiseByBarcodeIdAndAnalysisId(decimal barcodeId, long analysisId);
    bool CreateBarcodeAnalysise(BarcodeAnalysise barcodeAnalysise);
    bool DeleteBarcodeAnalysise(decimal barcodeId, long analysisId);
}
