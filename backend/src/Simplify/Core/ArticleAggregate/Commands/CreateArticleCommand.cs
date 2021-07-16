using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Simplify.Core.ArticleAggregate.Responses;

namespace Simplify.Core.ArticleAggregate.Commands
{
    public class CreateArticleCommand : IRequest<ArticleCreatedResponse>
    {
        public string Content { get; set; } = null!;
    }

    public class Asc : IRequest<ArticleCreatedResponse>
    {

    }

    public class AscHandler : IRequestHandler<Asc, ArticleCreatedResponse>
    {
        public Task<ArticleCreatedResponse> Handle(Asc request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}