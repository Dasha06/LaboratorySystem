using Microsoft.AspNetCore.Mvc;

namespace WebApi.Infrastructure;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<T> Execute<T>(Func<T> action)
    {
        try
        {
            return action();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    protected IActionResult Execute(Func<bool> action)
    {
        try
        {
            return action() ? Ok() : NotFound();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    protected IActionResult Execute(Action action)
    {
        try
        {
            action();
            return Ok();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
