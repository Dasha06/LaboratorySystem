using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure;

namespace WebApi.Controllers;

public class ReportsController : ApiControllerBase
{
    private readonly IReportRepository _repository;

    public ReportsController(IReportRepository repository) => _repository = repository;

    /// <summary>
    /// 1. Количество созданных заказов по работникам за промежуток времени.
    /// </summary>
    [HttpGet("orders-by-worker")]
    public ActionResult<List<KeyValuePair<string, int>>> GetOrdersCountByWorker(
        [FromQuery] DateTime from, [FromQuery] DateTime to) =>
        Execute(() => _repository.GetOrdersCountByWorker(from, to));

    /// <summary>
    /// 2. Список всех заказанных анализов за промежуток времени.
    /// </summary>
    [HttpGet("ordered-analyses")]
    public ActionResult<List<OrderedAnalysisRecord>> GetOrderedAnalysesBetween(
        [FromQuery] DateTime from, [FromQuery] DateTime to) =>
        Execute(() => _repository.GetOrderedAnalysesBetween(from, to));

    /// <summary>
    /// 3. Список заказанных анализов за промежуток времени по определенному ЛПУ.
    /// </summary>
    [HttpGet("ordered-analyses-by-lpu")]
    public ActionResult<List<OrderedAnalysisRecord>> GetOrderedAnalysesByLpu(
        [FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] long lpuId) =>
        Execute(() => _repository.GetOrderedAnalysesByLpu(from, to, lpuId));

    /// <summary>
    /// 4. Какие анализы доступны по договорам определенного ЛПУ.
    /// </summary>
    [HttpGet("available-analyses-by-lpu")]
    public ActionResult<List<AvailableAnalysisRecord>> GetAvailableAnalysesByLpu(
        [FromQuery] long lpuId) =>
        Execute(() => _repository.GetAvailableAnalysesByLpu(lpuId));

    /// <summary>
    /// Список всех ЛПУ для выбора.
    /// </summary>
    [HttpGet("lpus")]
    public ActionResult<List<Lpu>> GetAllLpus() =>
        Execute(() => _repository.GetAllLpus());
}