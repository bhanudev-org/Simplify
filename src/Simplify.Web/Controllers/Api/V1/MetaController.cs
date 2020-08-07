using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simplify.SeedWork.Commands;
using Simplify.Web.App.Commands;
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

        public async Task<IActionResult> Get()
        {
            var result2 = await _dispatcher.SendCommand(new OrderCommand
            {
                Id = Guid.NewGuid()
            });

            
            

            _logger.LogInformation("[API] [META] [GET] [{@headers}] : Called", Request.Headers);

            return Ok(new MetaViewModel());
        }
    }
}