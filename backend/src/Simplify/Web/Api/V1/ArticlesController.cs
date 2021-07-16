using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simplify.Core.ArticleAggregate;
using Simplify.Core.ArticleAggregate.Commands;
using Simplify.SeedWork.Storage;
using Simplify.Web.Models;

namespace Simplify.Web.Api.V1
{
    public class ArticlesController : ControllerBaseV1
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ArticlesController> _logger;
        private readonly IQueryStore<Article> _queries;

        public ArticlesController(ILogger<ArticlesController> logger, IMediator mediator ,IQueryStore<Article> queries)
        {
            _logger = logger;
            _mediator = mediator;
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
            var result = await _mediator.Send(new CreateArticleCommand { Content = model.Content });

            return Ok(result.Message);
        }
    }
}