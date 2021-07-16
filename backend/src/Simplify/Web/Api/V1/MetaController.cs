using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simplify.Web.Models;

namespace Simplify.Web.Api.V1
{
    public class MetaController : ControllerBaseV1
    {
        private readonly ILogger<MetaController> _logger;
        private readonly IMediator _mediator;

        public MetaController(ILogger<MetaController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public IActionResult Get()
        {
            _logger.LogInformation("[API] [META] [GET] [{@headers}] : Called", Request.Headers);

            return Ok(new MetaViewModel());
        }
    }
}