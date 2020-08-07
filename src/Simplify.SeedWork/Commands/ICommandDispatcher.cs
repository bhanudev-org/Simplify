using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace Simplify.SeedWork.Commands
{
    public interface ICommandDispatcher
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task SendCommand(object command);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task<Response<TCommandResponse>> SendCommand<TCommandResponse>(ICommand<TCommandResponse> command, CancellationToken cancellationToken = default) where TCommandResponse : class, ICommandResponse;
    }
}