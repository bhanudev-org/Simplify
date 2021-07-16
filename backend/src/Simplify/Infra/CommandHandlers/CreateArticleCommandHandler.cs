using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Simplify.Core.ArticleAggregate;
using Simplify.Core.ArticleAggregate.Commands;
using Simplify.Core.ArticleAggregate.Responses;
using Simplify.Infrastructure.Storage;

namespace Simplify.Infrastructure.CommandHandlers
{
    public class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand, ArticleCreatedResponse>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ILogger<CreateArticleCommandHandler> _logger;

        public CreateArticleCommandHandler(IArticleRepository articleRepository, ILogger<CreateArticleCommandHandler> logger)
        {
            _articleRepository = articleRepository;
            _logger = logger;
        }

        public async Task<ArticleCreatedResponse> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Command Called");

            var content = ArticleContent.Create(request.Content);
            var result = new ArticleCreatedResponse();
            if(content.IsSuccess)
            {
                var article = Article.Create(content.Value);

                if(article.IsSuccess)
                {
                    var stored = await _articleRepository.CreateAsync(article.Value, cancellationToken);

                    result.Updated(stored.Id, true);
                    return result;
                }

                result.Updated(message: article.Error);
                return result;
            }

            result.Updated(message: content.Error);
            return result;
        }
    }
}