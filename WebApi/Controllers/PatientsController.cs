using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class PatientsController : ApiControllerBase
{
    private readonly IPatientRepository _repository;

    public PatientsController(IPatientRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Patient>> GetAll() => Execute(_repository.GetAllPatients);

    [HttpGet("{patientId:long}")]
    public ActionResult<Patient> GetById(long patientId) =>
        Execute(() => _repository.GetPatientByPatientId(patientId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] PatientForm form) =>
        Execute(() => _repository.CreatePatient(FormEntityMapper.ToPatient(form)));

    [HttpPut("{patientId:long}")]
    [FormInput]
    public IActionResult Update(long patientId, [FromForm] PatientForm form)
    {
        var entity = FormEntityMapper.ToPatient(form);
        entity.PatientId = patientId;
        return Execute(() => _repository.UpdatePatient(entity));
    }

    [HttpDelete("{patientId:long}")]
    public IActionResult Delete(long patientId) => Execute(() => _repository.DeletePatient(patientId));
}
