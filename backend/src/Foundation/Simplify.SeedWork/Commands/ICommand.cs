using MediatR;

namespace Simplify.SeedWork
{
    public interface IBaseCommand : IRequest { }

    public interface ICommand : IRequest<Unit> { }

    public interface ICommand<out TCommandResponse> : IRequest<TCommandResponse> { }
}