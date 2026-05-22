using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class BarcodeAnalysesController : ApiControllerBase
{
    private readonly IBarcodeAnalysiseRepository _repository;

    public BarcodeAnalysesController(IBarcodeAnalysiseRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<BarcodeAnalysise>> GetAll() => Execute(_repository.GetAllBarcodeAnalysises);

    [HttpGet("by-barcode/{barcodeId:decimal}")]
    public ActionResult<List<BarcodeAnalysise>> GetByBarcode(decimal barcodeId) =>
        Execute(() => _repository.GetBarcodeAnalysisesByBarcodeId(barcodeId));

    [HttpGet("{barcodeId:decimal}/{analysisId:long}")]
    public ActionResult<BarcodeAnalysise> GetByKeys(decimal barcodeId, long analysisId) =>
        Execute(() => _repository.GetBarcodeAnalysiseByBarcodeIdAndAnalysisId(barcodeId, analysisId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] BarcodeAnalysiseForm form) =>
        Execute(() => _repository.CreateBarcodeAnalysise(FormEntityMapper.ToBarcodeAnalysise(form)));

    [HttpPut("{barcodeId:decimal}/{analysisId:long}")]
    [FormInput]
    public IActionResult Update(decimal barcodeId, long analysisId, [FromForm] BarcodeAnalysiseForm form)
    {
        var entity = FormEntityMapper.ToBarcodeAnalysise(form);
        entity.BarcodeId = barcodeId;
        entity.AnalysisId = analysisId;
        return Execute(() => _repository.UpdateBarcodeAnalysise(entity));
    }

    [HttpPatch("{barcodeId:decimal}/{analysisId:long}/result")]
    [FormInput]
    public IActionResult SetResult(decimal barcodeId, long analysisId, [FromForm] SetBarcodeResultForm form) =>
        Execute(() => _repository.SetBarcodeAnalysiseResultJson(barcodeId, analysisId, form.ResultJson));

    [HttpDelete("{barcodeId:decimal}/{analysisId:long}")]
    public IActionResult Delete(decimal barcodeId, long analysisId) =>
        Execute(() => _repository.DeleteBarcodeAnalysise(barcodeId, analysisId));
}
