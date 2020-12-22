using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Simplify.SeedWork
{
    public interface ICommandHandler<in TCommand,TCommandResponse> : IRequestHandler<TCommand,TCommandResponse>
        where TCommand : ICommand<TCommandResponse> { }

    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand { }

    public abstract class CommandHandlerAsync<TCommand> : AsyncRequestHandler<TCommand>
        where TCommand : IBaseCommand {}

    public abstract class CommandHandler<TCommand, TCommandResponse> : ICommandHandler<TCommand, TCommandResponse>
        where TCommand : ICommand<TCommandResponse>
    {
        public abstract Task<TCommandResponse> Handle(TCommand request, CancellationToken cancellationToken);
    }

    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public abstract Task<Unit> Handle(TCommand request, CancellationToken cancellationToken);
    }
}