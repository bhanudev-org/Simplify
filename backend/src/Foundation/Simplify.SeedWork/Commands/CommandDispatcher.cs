using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Simplify.SeedWork
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IMediator _mediator;

        public CommandDispatcher(IMediator mediator) => _mediator = mediator;

        public async Task SendCommand(object command, CancellationToken ct = default) =>
            await _mediator.Send(command, ct);

        public async Task<TCommandResponse> SendCommand<TCommandResponse>(ICommand<TCommandResponse> command, CancellationToken ct = default) where TCommandResponse : ICommandResponse =>
            await _mediator.Send(command, ct);
    }
}