using Simplify.Domain.ArticleAggregate.Responses;
using Simplify.SeedWork.Commands;

namespace Simplify.Domain.ArticleAggregate.Commands
{
    public class CreateArticleCommand : ICommand<ArticleCreatedResponse>
    {
        public string Content { get; set; } = null!;
    }
}