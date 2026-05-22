using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class TripodBarcodeMaterialRepositoryImpl : ITripodBarcodeMaterialRepository
{
    SystemdatabaseContext _context;
    public TripodBarcodeMaterialRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<TripodBarcodeMaterial> GetAllTripodBarcodeMaterials()
    {
        return _context.TripodBarcodeMaterials.ToList();
    }

    public List<TripodBarcodeMaterial> GetTripodBarcodeMaterialsByTripodId(long tripodId)
    {
        return _context.TripodBarcodeMaterials
            .Where(x => x.TripodId == tripodId)
            .Include(x => x.BarcodeMaterial)
            .ThenInclude(bm => bm.Material)
            .Include(x => x.BarcodeMaterial)
            .ThenInclude(bm => bm.AnalysisDep)
            .Include(x => x.BarcodeMaterial)
            .ThenInclude(bm => bm.BarcodeAnalysises)
            .ThenInclude(ba => ba.Analysis)
            .ToList();
    }

    public TripodBarcodeMaterial GetTripodBarcodeMaterialByTripodIdAndBarcodeMatId(long tripodId,
        decimal barcodeMatId)
    {
        return _context.TripodBarcodeMaterials.First(x => x.TripodId == tripodId && x.BarcodeMatId == barcodeMatId);
    }

    public bool CreateTripodBarcodeMaterial(TripodBarcodeMaterial tripodBarcodeMaterial)
    {
        _context.TripodBarcodeMaterials.Add(tripodBarcodeMaterial);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateTripodBarcodeMaterial(TripodBarcodeMaterial tripodBarcodeMaterial)
    {
        var existing = _context.TripodBarcodeMaterials.First(x =>
            x.TripodId == tripodBarcodeMaterial.TripodId && x.BarcodeMatId == tripodBarcodeMaterial.BarcodeMatId);
        existing.AnalysisDepId = tripodBarcodeMaterial.AnalysisDepId;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteTripodBarcodeMaterial(long tripodId, decimal barcodeMatId)
    {
        var tripodBarcodeMaterial = _context.TripodBarcodeMaterials.First(x =>
            x.TripodId == tripodId && x.BarcodeMatId == barcodeMatId);
        _context.TripodBarcodeMaterials.Remove(tripodBarcodeMaterial);
        _context.SaveChanges();
        return true;
    }
}
