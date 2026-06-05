using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class MaterialsController : ApiControllerBase
{
    private readonly IMaterialRepository _repository;

    public MaterialsController(IMaterialRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Material>> GetAll() => Execute(_repository.GetAllMaterials);

    [HttpGet("{materialId:int}")]
    public ActionResult<Material> GetById(int materialId) =>
        Execute(() => _repository.GetMaterialByMaterialId(materialId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] MaterialForm form) =>
        Execute(() => _repository.CreateMaterial(FormEntityMapper.ToMaterial(form)));

    [HttpPut("{materialId:int}")]
    [FormInput]
    public IActionResult Update(int materialId, [FromForm] MaterialForm form)
    {
        var entity = FormEntityMapper.ToMaterial(form);
        entity.MaterialId = materialId;
        return Execute(() => _repository.UpdateMaterial(entity));
    }

    [HttpDelete("{materialId:int}")]
    public IActionResult Delete(int materialId) => Execute(() => _repository.DeleteMaterial(materialId));
}
