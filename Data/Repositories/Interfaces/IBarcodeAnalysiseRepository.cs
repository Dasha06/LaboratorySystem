using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IBarcodeAnalysiseRepository
{
    List<BarcodeAnalysise> GetAllBarcodeAnalysises();
    List<BarcodeAnalysise> GetBarcodeAnalysisesByBarcodeId(decimal barcodeId);
    BarcodeAnalysise GetBarcodeAnalysiseByBarcodeIdAndAnalysisId(decimal barcodeId, long analysisId);
    bool CreateBarcodeAnalysise(BarcodeAnalysise barcodeAnalysise);
    bool UpdateBarcodeAnalysise(BarcodeAnalysise barcodeAnalysise);
    bool SetBarcodeAnalysiseResultJson(decimal barcodeId, long analysisId, string? resultJson);
    bool DeleteBarcodeAnalysise(decimal barcodeId, long analysisId);
}
