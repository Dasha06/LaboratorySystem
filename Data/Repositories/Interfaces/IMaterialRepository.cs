using Data.Models;

namespace Data.Repositories.Interfaces;

public interface IMaterialRepository
{
    List<Material> GetAllMaterials();
    Material GetMaterialByMaterialId(int materialId);
    bool CreateMaterial(Material material);
    bool UpdateMaterial(Material material);
    bool DeleteMaterial(int materialId);
}