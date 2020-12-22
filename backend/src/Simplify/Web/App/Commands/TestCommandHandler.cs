using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Simplify.SeedWork;

namespace Simplify.Web.App.Commands
{
    public class TestCommandHandler : ICommandHandler<OrderCommand,OrderResponse>
    {
        public async Task Consume(ConsumeContext<OrderCommand> context)
        {
            await context.RespondAsync<OrderResponse>(new { context.Message.Id, Note = "Hello" });
        }

        public async Task<OrderResponse> Handle(OrderCommand request, CancellationToken cancellationToken) => null;
    }
}