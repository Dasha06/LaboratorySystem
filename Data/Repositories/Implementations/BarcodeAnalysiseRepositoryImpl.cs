using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public List<BarcodeAnalysise> GetBarcodeAnalysisesByBarcodeId(decimal barcodeId)
    {
        return _context.BarcodeAnalysises
            .Include(x => x.Analysis)
            .Where(x => x.BarcodeId == barcodeId)
            .ToList();
    }

    public BarcodeAnalysise GetBarcodeAnalysiseByBarcodeIdAndAnalysisId(decimal barcodeId, long analysisId)
    {
        return _context.BarcodeAnalysises
            .Include(x => x.Analysis)
            .First(x => x.BarcodeId == barcodeId && x.AnalysisId == analysisId);
    }

    public bool CreateBarcodeAnalysise(BarcodeAnalysise barcodeAnalysise)
    {
        _context.BarcodeAnalysises.Add(barcodeAnalysise);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateBarcodeAnalysise(BarcodeAnalysise barcodeAnalysise)
    {
        var existing = _context.BarcodeAnalysises.First(x =>
            x.BarcodeId == barcodeAnalysise.BarcodeId && x.AnalysisId == barcodeAnalysise.AnalysisId);
        existing.AnalysisDepId = barcodeAnalysise.AnalysisDepId;
        existing.Result = barcodeAnalysise.Result;
        _context.SaveChanges();
        return true;
    }

    public bool SetBarcodeAnalysiseResultJson(decimal barcodeId, long analysisId, string? resultJson)
    {
        var existing = _context.BarcodeAnalysises.First(x =>
            x.BarcodeId == barcodeId && x.AnalysisId == analysisId);
        existing.Result = resultJson;
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
