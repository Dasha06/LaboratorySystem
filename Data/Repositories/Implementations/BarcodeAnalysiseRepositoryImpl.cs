using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class BarcodeAnalysiseRepositoryImpl : IBarcodeAnalysiseRepository
{
    SystemdatabaseContext _context;
    public BarcodeAnalysiseRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<BarcodeAnalysise> GetAllBarcodeAnalysises()
    {
        return _context.BarcodeAnalysises.ToList();
    }
//maybe? TODO: need to create another one but with only barcode for getting every analysis, add result
    public BarcodeAnalysise GetBarcodeAnalysiseByBarcodeIdAndAnalysisId(decimal barcodeId, long analysisId)
    {
        return _context.BarcodeAnalysises.First(x => x.BarcodeId == barcodeId && x.AnalysisId == analysisId);
    }

    public bool CreateBarcodeAnalysise(BarcodeAnalysise barcodeAnalysise)
    {
        _context.BarcodeAnalysises.Add(barcodeAnalysise);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteBarcodeAnalysise(decimal barcodeId, long analysisId)
    {
        var barcodeAnalysise = _context.BarcodeAnalysises.First(x => x.BarcodeId == barcodeId && x.AnalysisId == analysisId);
        _context.BarcodeAnalysises.Remove(barcodeAnalysise);
        _context.SaveChanges();
        return true;
    }
}
