using Simplify.SeedWork.Domain;

namespace Simplify.Domain.ArticleAggregate
{
    public class ArticleVisibility : Enumeration
    {
        public static readonly ArticleVisibility Public = new ArticleVisibility(1, "PUBLIC");
        public static readonly ArticleVisibility Private = new ArticleVisibility(1, "PRIVATE");
        public static readonly ArticleVisibility Protected = new ArticleVisibility(1, "PROTECTED");

        public ArticleVisibility(int value, string name) : base(value, name) { }
    }
}