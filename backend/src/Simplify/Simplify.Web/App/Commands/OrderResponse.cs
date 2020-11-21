using System;
using Simplify.SeedWork;

namespace Simplify.Web.App.Commands
{
    public interface OrderResponse : ICommandResponse
    {
        public string Note { get; }

        public Guid Id { get; }
    }
}