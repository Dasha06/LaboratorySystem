using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class PatientChangesController : ApiControllerBase
{
    private readonly IPatientChangeRepository _repository;

    public PatientChangesController(IPatientChangeRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<PatientChange>> GetAll() => Execute(_repository.GetAllPatientChanges);

    [HttpGet("{patientId:long}/{workerId:int}")]
    public ActionResult<PatientChange> GetByKeys(long patientId, int workerId,
        [FromQuery] DateTime patientChangeTime) =>
        Execute(() => _repository.GetPatientChangeByPatientIdAndWorkerIdAndPatientChangeTime(
            patientId, workerId, patientChangeTime));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] PatientChangeForm form) =>
        Execute(() => _repository.CreatePatientChange(FormEntityMapper.ToPatientChange(form)));

    [HttpDelete("{patientId:long}/{workerId:int}")]
    public IActionResult Delete(long patientId, int workerId, [FromQuery] DateTime patientChangeTime) =>
        Execute(() => _repository.DeletePatientChange(patientId, workerId, patientChangeTime));
}
