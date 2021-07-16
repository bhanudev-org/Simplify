using System;
using Simplify.SeedWork;

namespace Simplify.Core.ArticleAggregate.Responses
{
    public class ArticleCreatedResponse
    {
        public ArticleCreatedResponse()
        {
            IsSuccess = false;
            Message = string.Empty;
        }
        public Guid? Id { get; private set; }

        public bool IsSuccess { get; private set; }

        public string? Message { get; private set; }

        public void Updated(Guid? id = default, bool isSuccess = default, string? message = default)
        {
            Id = id;
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}