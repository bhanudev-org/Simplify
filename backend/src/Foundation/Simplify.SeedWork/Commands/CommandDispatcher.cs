using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Mediator;

namespace Simplify.SeedWork.Commands
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IMediator _mediator;

        public CommandDispatcher(IMediator mediator) => _mediator = mediator;

        public async Task SendCommand(object command) => await _mediator.Send(command);

        public async Task<Response<TCommandResponse>> SendCommand<TCommandResponse>(ICommand<TCommandResponse> command, CancellationToken cancellationToken = default) where TCommandResponse : class, ICommandResponse
        {
            var client = _mediator.CreateRequestClient<ICommand<TCommandResponse>>();

            return await client.GetResponse<TCommandResponse>(command, cancellationToken);
        }
    }
}