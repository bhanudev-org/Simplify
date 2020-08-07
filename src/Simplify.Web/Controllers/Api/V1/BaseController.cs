using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace Simplify.Web.Controllers.Api.V1
{
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BaseController : ControllerBase { }
}