using MassTransit;

namespace Simplify.SeedWork.Commands
{
    public interface ICommandHandler<in TCommand> : IConsumer<TCommand> where TCommand : class, ICommand { }
}