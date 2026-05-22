using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class DoctorsController : ApiControllerBase
{
    private readonly IDoctorRepository _repository;

    public DoctorsController(IDoctorRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Doctor>> GetAll() => Execute(_repository.GetAllDoctors);

    [HttpGet("{docId:long}")]
    public ActionResult<Doctor> GetById(long docId) => Execute(() => _repository.GetDoctorByDocId(docId));

    [HttpGet("by-lpu/{lpuId:long}")]
    public ActionResult<List<Doctor>> GetByLpu(long lpuId) => Execute(() => _repository.GetDoctorsByLpuId(lpuId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] DoctorForm form) =>
        Execute(() => _repository.CreateDoctor(FormEntityMapper.ToDoctor(form)));

    [HttpPut("{docId:long}")]
    [FormInput]
    public IActionResult Update(long docId, [FromForm] DoctorForm form)
    {
        var entity = FormEntityMapper.ToDoctor(form);
        entity.DocId = docId;
        return Execute(() => _repository.UpdateDoctor(entity));
    }

    [HttpDelete("{docId:long}")]
    public IActionResult Delete(long docId) => Execute(() => _repository.DeleteDoctor(docId));
}
