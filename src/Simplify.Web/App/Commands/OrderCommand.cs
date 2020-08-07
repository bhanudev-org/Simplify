using System;
using Simplify.SeedWork.Commands;

namespace Simplify.Web.App.Commands
{
    public class OrderCommand : ICommand<OrderResponse>
    {
        public Guid Id { get; set; }
    }
}