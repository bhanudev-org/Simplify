using CSharpFunctionalExtensions;

namespace Simplify.Core.ArticleAggregate
{
    public class ArticleContent : SimpleValueObject<string>
    {
        private const string _displayText = "Article Content";

        private ArticleContent(string value) : base(value) { }

        public static Result<ArticleContent> Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                return Result.Failure<ArticleContent>($"{_displayText} cannot be empty");

            if(value.Length > 50000)
                return Result.Failure<ArticleContent>($"{_displayText} is too long");

            return Result.Success(new ArticleContent(value));
        }
    }
}