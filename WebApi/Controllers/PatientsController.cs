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
    public IActionResult Create([FromForm] PatientForm form)
    {
        var patient = FormEntityMapper.ToPatient(form);
        var result = _repository.CreatePatient(patient);
        if (result && form.WorkerId.HasValue)
        {
            _repository.CreatePatientChange(new PatientChange
            {
                PatientId = patient.PatientId,
                WorkerId = form.WorkerId.Value,
                PatientChangeTime = DateTime.Now,
                TypeId = 1 // 1 = создание
            });
        }
        return Execute(() => result);
    }

    [HttpPut("{patientId:long}")]
    [FormInput]
    public IActionResult Update(long patientId, [FromForm] PatientForm form)
    {
        var entity = FormEntityMapper.ToPatient(form);
        entity.PatientId = patientId;
        var result = _repository.UpdatePatient(entity);
        if (result && form.WorkerId.HasValue)
        {
            _repository.CreatePatientChange(new PatientChange
            {
                PatientId = patientId,
                WorkerId = form.WorkerId.Value,
                PatientChangeTime = DateTime.Now,
                TypeId = 2 // 2 = изменение
            });
        }
        return Execute(() => result);
    }

    [HttpDelete("{patientId:long}")]
    public IActionResult Delete(long patientId) => Execute(() => _repository.DeletePatient(patientId));
}
