using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simplify.Domain.ArticleAggregate.Commands;
using Simplify.SeedWork.Commands;
using Simplify.Web.App.Commands;
using Simplify.Web.Models;

namespace Simplify.Web.Controllers.Api.V1
{
    public class ArticlesController : BaseController
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(ILogger<ArticlesController> logger, ICommandDispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogDebug("[API] [ARTICLE] [GET]", Request.Headers);

            var result = await _dispatcher.SendCommand(new OrderCommand {Id = Guid.NewGuid()});

            return Ok(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ArticleViewModel model)
        {
            var result = await _dispatcher.SendCommand(new CreateArticleCommand {Content = model.Content});

            return Ok(result.Message);
        }
    }
}