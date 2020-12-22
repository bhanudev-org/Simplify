using System;
using CSharpFunctionalExtensions;
using NodaTime;
using Simplify.SeedWork.Domain;
using Simplify.SeedWork.Entities;

namespace Simplify.Core.ArticleAggregate
{
    public class Article : AggregateRoot, IEntityWithAudit
    {
        public Article(ArticleContent content)
        {
            Content = content;
            Status = ArticleStatus.Draft;
            Visibility = ArticleVisibility.Private;
        }

        public ArticleContent Content { get; private set; }

        public ArticleStatus Status { get; private set; }

        public ArticleVisibility Visibility { get; private set; }
        
        public void UpdateContent(ArticleContent content) => Content = content;
        public void UpdateStatus(ArticleStatus status) => Status = status;
        public void UpdateVisibility(ArticleVisibility visibility) => Visibility = visibility;

        public static Result<Article> Create(ArticleContent content)
        {
            var article = new Article(content);
            article.Created(Guid.Empty); // TODO
            return Result.Success(article);
        }

        #region Audit Fields

        public Guid CreatedById { get; set; }

        public Instant CreatedOn { get; set; }

        public Guid LastModifiedById { get; set; }

        public Instant LastModifiedOn { get; set; }

        #endregion
    }
}