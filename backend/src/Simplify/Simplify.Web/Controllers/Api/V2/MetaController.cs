using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simplify.Web.Models;

namespace Simplify.Web.Controllers.Api.V2
{
    public class MetaController : BaseController
    {
        private readonly ILogger<MetaController> _logger;

        public MetaController(ILogger<MetaController> logger) => _logger = logger;

        public IActionResult Get()
        {
            _logger.LogInformation("[API] [META] [GET] [{@headers}] : Called", Request.Headers);

            return Ok(new MetaViewModel());
        }
    }
}