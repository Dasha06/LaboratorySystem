using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;
using WebApi.Mapping;
using WebApi.Models.Forms;
using WebApi.Models.Responses;

namespace WebApi.Controllers;

public class TripodsController : ApiControllerBase
{
    private readonly ITripodRepository _repository;
    private readonly ITripodBarcodeMaterialRepository _tripodBarcodeMaterialRepository;

    public TripodsController(
        ITripodRepository repository,
        ITripodBarcodeMaterialRepository tripodBarcodeMaterialRepository)
    {
        _repository = repository;
        _tripodBarcodeMaterialRepository = tripodBarcodeMaterialRepository;
    }

    [HttpGet]
    public ActionResult<List<Tripod>> GetAll() => Execute(_repository.GetAllTripods);

    [HttpGet("{tripodId:long}")]
    public ActionResult<Tripod> GetById(long tripodId) => Execute(() => _repository.GetTripodByTripodId(tripodId));

    [HttpGet("{tripodId:long}/worksheets")]
    public ActionResult<List<WorksheetRowDto>> GetWorksheets(long tripodId) =>
        Execute(() =>
        {
            _ = _repository.GetTripodByTripodId(tripodId);
            var items = _tripodBarcodeMaterialRepository.GetTripodBarcodeMaterialsByTripodId(tripodId);
            return WorksheetMapper.ToWorksheetRows(items);
        });

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
