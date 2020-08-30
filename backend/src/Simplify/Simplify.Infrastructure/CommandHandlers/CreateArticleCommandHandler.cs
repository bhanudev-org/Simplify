using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Simplify.Core;
using Simplify.Domain.ArticleAggregate;
using Simplify.Domain.ArticleAggregate.Commands;
using Simplify.Domain.ArticleAggregate.Responses;
using Simplify.Infrastructure.Storage;
using Simplify.SeedWork.Commands;

namespace Simplify.Infrastructure.CommandHandlers
{
    public class CreateArticleCommandHandler : ICommandHandler<CreateArticleCommand>
    {
        private readonly ILogger<CreateArticleCommandHandler> _logger;
        private readonly IArticleRepository _articleRepository;

        public CreateArticleCommandHandler(IArticleRepository articleRepository, ILogger<CreateArticleCommandHandler> logger)
        {
            _articleRepository = articleRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateArticleCommand> context)
        {
            _logger.LogDebug("Command Called");

            var content = ArticleContent.Create(context.Message.Content);

            if(content.IsSuccess)
            {
                var article = Article.Create(content.Value);

                if(article.IsSuccess)
                {
                    var stored = await _articleRepository.CreateAsync(article.Value);

                    await context.RespondAsync<ArticleCreatedResponse>(new { stored.Id, Success = stored.IsAdded });
                }
                else
                {
                    await context.RespondAsync<ArticleCreatedResponse>(new { Id = Guid.Empty, Success = false, Message = article.Error });
                }
            }
            else
            {
                await context.RespondAsync<ArticleCreatedResponse>(new { Id = Guid.Empty, Success = false, Message = content.Error });
            }

            
        }
    }
}