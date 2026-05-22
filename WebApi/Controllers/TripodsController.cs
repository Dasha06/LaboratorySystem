using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;

namespace WebApi.Controllers;

public class TripodsController : ApiControllerBase
{
    private readonly ITripodRepository _repository;

    public TripodsController(ITripodRepository repository) => _repository = repository;

    [HttpGet]
    public ActionResult<List<Tripod>> GetAll() => Execute(_repository.GetAllTripods);

    [HttpGet("{tripodId:long}")]
    public ActionResult<Tripod> GetById(long tripodId) => Execute(() => _repository.GetTripodByTripodId(tripodId));

    [HttpPost]
    [FormInput]
    public IActionResult Create([FromForm] TripodForm form) =>
        Execute(() => _repository.CreateTripod(FormEntityMapper.ToTripod(form)));

    [HttpPut("{tripodId:long}")]
    [FormInput]
    public IActionResult Update(long tripodId, [FromForm] TripodForm form)
    {
        var entity = FormEntityMapper.ToTripod(form);
        entity.TripodId = tripodId;
        return Execute(() => _repository.UpdateTripod(entity));
    }

    [HttpDelete("{tripodId:long}")]
    public IActionResult Delete(long tripodId) => Execute(() => _repository.DeleteTripod(tripodId));
}
