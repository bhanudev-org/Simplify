using System;
using Simplify.SeedWork.Commands;

#nullable disable

namespace Simplify.Domain.ArticleAggregate.Responses
{
    public class ArticleCreatedResponse : ICommandResponse
    {
        public Guid Id { get; private set; }

        public bool Success { get; private set; }

        public string Message { get; private set; }
    }
}