using System;
using Simplify.SeedWork.Commands;

namespace Simplify.Web.App.Commands
{
    public interface IOrderCommand : ICommand
    {
        public Guid Id { get; }
    }
}