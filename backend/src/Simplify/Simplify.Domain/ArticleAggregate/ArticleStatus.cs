using Pro.Enum;

namespace Simplify.Domain.ArticleAggregate
{
    public class ArticleStatus : Enumeration
    {
        public static readonly ArticleStatus Draft = new ArticleStatus(1, "DRAFT");
        public static readonly ArticleStatus Scheduled = new ArticleStatus(2, "SCHEDULED");
        public static readonly ArticleStatus Published = new ArticleStatus(3, "PUBLISHED");

        private ArticleStatus(int value, string name) : base(value, name) { }
    }
}