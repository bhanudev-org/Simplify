using System.Threading.Tasks;
using MassTransit;
using Simplify.SeedWork.Commands;

namespace Simplify.Web.App.Commands
{
    public class TestCommandHandler : ICommandHandler<OrderCommand>
    {
        public async Task Consume(ConsumeContext<OrderCommand> context)
        {
            await context.RespondAsync<OrderResponse>(new { context.Message.Id, Note = "Hello" });
        }
    }
}