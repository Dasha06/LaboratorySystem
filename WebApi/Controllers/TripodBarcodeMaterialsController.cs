using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class TripodBarcodeMaterialsController : ApiControllerBase
{
    private readonly ITripodBarcodeMaterialRepository _repository;

    public TripodBarcodeMaterialsController(ITripodBarcodeMaterialRepository repository) =>
        _repository = repository;

    [HttpGet]
    public ActionResult<List<TripodBarcodeMaterial>> GetAll() => Execute(_repository.GetAllTripodBarcodeMaterials);

    [HttpGet("by-tripod/{tripodId:long}")]
    public ActionResult<List<TripodBarcodeMaterial>> GetByTripod(long tripodId) =>
        Execute(() => _repository.GetTripodBarcodeMaterialsByTripodId(tripodId));

    [HttpGet("{tripodId:long}/{barcodeMatId:decimal}")]
    public ActionResult<TripodBarcodeMaterial> GetByKeys(long tripodId, decimal barcodeMatId) =>
        Execute(() => _repository.GetTripodBarcodeMaterialByTripodIdAndBarcodeMatId(tripodId, barcodeMatId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] TripodBarcodeMaterialForm form) =>
        Execute(() => _repository.CreateTripodBarcodeMaterial(FormEntityMapper.ToTripodBarcodeMaterial(form)));

    [HttpPut("{tripodId:long}/{barcodeMatId:decimal}")]
    [FormInput]
    public IActionResult Update(long tripodId, decimal barcodeMatId, [FromForm] TripodBarcodeMaterialForm form)
    {
        var entity = FormEntityMapper.ToTripodBarcodeMaterial(form);
        entity.TripodId = tripodId;
        entity.BarcodeMatId = barcodeMatId;
        return Execute(() => _repository.UpdateTripodBarcodeMaterial(entity));
    }

    [HttpDelete("{tripodId:long}/{barcodeMatId:decimal}")]
    public IActionResult Delete(long tripodId, decimal barcodeMatId) =>
        Execute(() => _repository.DeleteTripodBarcodeMaterial(tripodId, barcodeMatId));
}
