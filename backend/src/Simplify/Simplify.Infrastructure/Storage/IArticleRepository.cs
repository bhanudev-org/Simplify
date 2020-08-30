using Simplify.Domain.ArticleAggregate;
using Simplify.SeedWork.Storage;

namespace Simplify.Infrastructure.Storage
{
    public interface IArticleRepository : ICommandStorage<Article>, IQueryStorage<Article>
    {

    }
}