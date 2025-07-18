using Microsoft.AspNetCore.Mvc;

namespace BE.API.Controllers
{
    [ApiController]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
