using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Implementations;

public class BarcodeComplexRepositoryImpl : IBarcodeComplexRepository
{
    private readonly SystemdatabaseContext _context;

    public BarcodeComplexRepositoryImpl(SystemdatabaseContext context)
    {
        _context = context;
    }

    public List<BarcodeComplex> GetAllBarcodeComplexes()
    {
        return _context.BarcodeComplexes
            .Include(bc => bc.Complex)
            .Include(bc => bc.BarcodeMaterial)
            .ToList();
    }

    public List<BarcodeComplex> GetBarcodeComplexesByBarcodeMatIdAndAnalysisDepId(decimal barcodeMatId,
        int analysisDepId)
    {
        return _context.BarcodeComplexes
            .Include(bc => bc.Complex)
            .Where(bc => bc.BarcodeMatId == barcodeMatId && bc.AnalysisDepId == analysisDepId)
            .ToList();
    }

    public BarcodeComplex GetBarcodeComplexByKeys(decimal barcodeMatId, int complexId)
    {
        return _context.BarcodeComplexes
            .Include(bc => bc.Complex)
            .Include(bc => bc.BarcodeMaterial)
            .First(bc => bc.BarcodeMatId == barcodeMatId && bc.ComplexId == complexId);
    }

    public bool CreateBarcodeComplex(BarcodeComplex barcodeComplex)
    {
        _context.BarcodeComplexes.Add(barcodeComplex);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateBarcodeComplex(BarcodeComplex barcodeComplex)
    {
        var existing = _context.BarcodeComplexes.First(bc =>
            bc.BarcodeMatId == barcodeComplex.BarcodeMatId && bc.ComplexId == barcodeComplex.ComplexId);
        existing.AnalysisDepId = barcodeComplex.AnalysisDepId;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteBarcodeComplex(decimal barcodeMatId, int complexId)
    {
        var entity = _context.BarcodeComplexes.First(bc =>
            bc.BarcodeMatId == barcodeMatId && bc.ComplexId == complexId);
        _context.BarcodeComplexes.Remove(entity);
        _context.SaveChanges();
        return true;
    }
}
