using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class BarcodeComplexesController : ApiControllerBase
{
    private readonly IBarcodeComplexRepository _repository;

    public BarcodeComplexesController(IBarcodeComplexRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<BarcodeComplex>> GetAll() => Execute(_repository.GetAllBarcodeComplexes);

    [HttpGet("by-barcode/{barcodeMatId:decimal}/{analysisDepId:int}")]
    public ActionResult<List<BarcodeComplex>> GetByBarcode(decimal barcodeMatId, int analysisDepId) =>
        Execute(() => _repository.GetBarcodeComplexesByBarcodeMatIdAndAnalysisDepId(barcodeMatId, analysisDepId));

    [HttpGet("{barcodeMatId:decimal}/{complexId:int}")]
    public ActionResult<BarcodeComplex> GetByKeys(decimal barcodeMatId, int complexId) =>
        Execute(() => _repository.GetBarcodeComplexByKeys(barcodeMatId, complexId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] BarcodeComplexForm form) =>
        Execute(() => _repository.CreateBarcodeComplex(FormEntityMapper.ToBarcodeComplex(form)));

    [HttpPut("{barcodeMatId:decimal}/{complexId:int}")]
    [FormInput]
    public IActionResult Update(decimal barcodeMatId, int complexId, [FromForm] BarcodeComplexForm form)
    {
        var entity = FormEntityMapper.ToBarcodeComplex(form);
        entity.BarcodeMatId = barcodeMatId;
        entity.ComplexId = complexId;
        return Execute(() => _repository.UpdateBarcodeComplex(entity));
    }

    [HttpDelete("{barcodeMatId:decimal}/{complexId:int}")]
    public IActionResult Delete(decimal barcodeMatId, int complexId) =>
        Execute(() => _repository.DeleteBarcodeComplex(barcodeMatId, complexId));
}
