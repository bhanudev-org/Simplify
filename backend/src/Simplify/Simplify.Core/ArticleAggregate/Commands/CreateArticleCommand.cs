using Simplify.Core.ArticleAggregate.Responses;
using Simplify.SeedWork.Commands;

namespace Simplify.Core.ArticleAggregate.Commands
{
    public class CreateArticleCommand : ICommand<ArticleCreatedResponse>
    {
        public string Content { get; set; } = null!;
    }
}