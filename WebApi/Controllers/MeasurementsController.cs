using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class MeasurementsController : ApiControllerBase
{
    private readonly IMeasurementRepository _repository;

    public MeasurementsController(IMeasurementRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Measurement>> GetAll() => Execute(_repository.GetAllMeasurements);

    [HttpGet("{measurementId:int}")]
    public ActionResult<Measurement> GetById(int measurementId) =>
        Execute(() => _repository.GetMeasurementByMeasurementId(measurementId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] MeasurementForm form) =>
        Execute(() => _repository.CreateMeasurement(FormEntityMapper.ToMeasurement(form)));

    [HttpPut("{measurementId:int}")]
    [FormInput]
    public IActionResult Update(int measurementId, [FromForm] MeasurementForm form)
    {
        var entity = FormEntityMapper.ToMeasurement(form);
        entity.MeasurementId = measurementId;
        return Execute(() => _repository.UpdateMeasurement(entity));
    }

    [HttpDelete("{measurementId:int}")]
    public IActionResult Delete(int measurementId) => Execute(() => _repository.DeleteMeasurement(measurementId));
}
