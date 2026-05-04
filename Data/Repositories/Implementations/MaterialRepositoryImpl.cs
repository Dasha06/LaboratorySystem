using Data.Models;
using Data.Repositories.Interfaces;

namespace Data.Repositories.Implementations;

public class MaterialRepositoryImpl : IMaterialRepository
{
    SystemdatabaseContext _context;
    public MaterialRepositoryImpl(SystemdatabaseContext remoteDatabaseContext)
    {
        _context = remoteDatabaseContext;
    }

    public List<Material> GetAllMaterials()
    {
        return _context.Materials.ToList();
    }

    public Material GetMaterialByMaterialId(int materialId)
    {
        return _context.Materials.First(x => x.MaterialId == materialId);
    }

    public bool CreateMaterial(Material material)
    {
        _context.Materials.Add(material);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteMaterial(int materialId)
    {
        var material = _context.Materials.First(x => x.MaterialId == materialId);
        _context.Materials.Remove(material);
        _context.SaveChanges();
        return true;
    }
}
