using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Simplify.Web.Controllers.Api.V2
{
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [ApiController]
    [Route("api/v2/[controller]")]
    public class BaseController : ControllerBase { }
}