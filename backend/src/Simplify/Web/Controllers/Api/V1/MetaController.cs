using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simplify.SeedWork;
using Simplify.Web.Models;

namespace Simplify.Web.Controllers.Api.V1
{
    public class MetaController : BaseController
    {
        private readonly ILogger<MetaController> _logger;
        private readonly ICommandDispatcher _dispatcher;

        public MetaController(ILogger<MetaController> logger, ICommandDispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        public IActionResult Get()
        {
            _logger.LogInformation("[API] [META] [GET] [{@headers}] : Called", Request.Headers);

            return Ok(new MetaViewModel());
        }
    }
}