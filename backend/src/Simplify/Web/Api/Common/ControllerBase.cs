using System.Net.Mime;

namespace Simplify.Web.Api
{
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [ApiController]
    public abstract class SimplifyControllerBase : ControllerBase
    {
    }

    [Route("api/v1/[controller]")]
    public abstract class ControllerBaseV1 : SimplifyControllerBase { }

    [Route("api/v2/[controller]")]
    public abstract class ControllerBaseV2 : SimplifyControllerBase { }
}
