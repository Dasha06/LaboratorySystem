using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class BarcodeMaterialRepositoryImpl : IBarcodeMaterialRepository
{
    SystemdatabaseContext _context;
    public BarcodeMaterialRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<BarcodeMaterial> GetAllBarcodeMaterials()
    {
        return _context.BarcodeMaterials.ToList();
    }

    public BarcodeMaterial GetBarcodeMaterialByBarcodeMatIdAndAnalysisDepId(decimal barcodeMatId, int analysisDepId)
    {
        return _context.BarcodeMaterials.First(x => x.BarcodeMatId == barcodeMatId && x.AnalysisDepId == analysisDepId);
    }

    public bool CreateBarcodeMaterial(BarcodeMaterial barcodeMaterial)
    {
        _context.BarcodeMaterials.Add(barcodeMaterial);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteBarcodeMaterial(decimal barcodeMatId, int analysisDepId)
    {
        var barcodeMaterial = _context.BarcodeMaterials.First(x => x.BarcodeMatId == barcodeMatId && x.AnalysisDepId == analysisDepId);
        _context.BarcodeMaterials.Remove(barcodeMaterial);
        _context.SaveChanges();
        return true;
    }
}
