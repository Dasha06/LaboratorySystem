using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class BarcodeMaterialsController : ApiControllerBase
{
    private readonly IBarcodeMaterialRepository _repository;

    public BarcodeMaterialsController(IBarcodeMaterialRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<BarcodeMaterial>> GetAll() => Execute(_repository.GetAllBarcodeMaterials);

    [HttpGet("by-order/{orderId:long}")]
    public ActionResult<List<BarcodeMaterial>> GetByOrder(long orderId) =>
        Execute(() => _repository.GetBarcodeMaterialsByOrderId(orderId));

    [HttpGet("{barcodeMatId:decimal}")]
    public ActionResult<BarcodeMaterial?> GetByBarcode(decimal barcodeMatId) =>
        Execute(() => _repository.GetBarcodeMaterialByBarcodeMatId(barcodeMatId));

    [HttpGet("{barcodeMatId:decimal}/{analysisDepId:int}")]
    public ActionResult<BarcodeMaterial> GetByKeys(decimal barcodeMatId, int analysisDepId) =>
        Execute(() => _repository.GetBarcodeMaterialByBarcodeMatIdAndAnalysisDepId(barcodeMatId, analysisDepId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] BarcodeMaterialForm form) =>
        Execute(() => _repository.CreateBarcodeMaterial(FormEntityMapper.ToBarcodeMaterial(form)));

    [HttpPut("{barcodeMatId:decimal}/{analysisDepId:int}")]
    [FormInput]
    public IActionResult Update(decimal barcodeMatId, int analysisDepId, [FromForm] BarcodeMaterialForm form)
    {
        var entity = FormEntityMapper.ToBarcodeMaterial(form);
        entity.BarcodeMatId = barcodeMatId;
        entity.AnalysisDepId = analysisDepId;
        return Execute(() => _repository.UpdateBarcodeMaterial(entity));
    }

    [HttpDelete("{barcodeMatId:decimal}/{analysisDepId:int}")]
    public IActionResult Delete(decimal barcodeMatId, int analysisDepId) =>
        Execute(() => _repository.DeleteBarcodeMaterial(barcodeMatId, analysisDepId));
}
