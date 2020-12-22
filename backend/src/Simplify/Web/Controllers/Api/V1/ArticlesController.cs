using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simplify.Core.ArticleAggregate;
using Simplify.Core.ArticleAggregate.Commands;
using Simplify.SeedWork;
using Simplify.SeedWork.Storage;
using Simplify.Web.Models;

namespace Simplify.Web.Controllers.Api.V1
{
    public class ArticlesController : BaseController
    {
        private readonly ICommandDispatcher _dispatcher;
        private readonly ILogger<ArticlesController> _logger;
        private readonly IQueryStore<Article> _queries;

        public ArticlesController(ILogger<ArticlesController> logger, ICommandDispatcher dispatcher, IQueryStore<Article> queries)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            _queries = queries;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id = default)
        {
            _logger.LogDebug("[API] [ARTICLE] [GET]", Request.Headers);

            return id == default ? Ok(await _queries.GetAsync()) : Ok(await _queries.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(ArticleViewModel model)
        {
            var result = await _dispatcher.SendCommand(new CreateArticleCommand {Content = model.Content});

            return Ok(result.Message);
        }
    }
}