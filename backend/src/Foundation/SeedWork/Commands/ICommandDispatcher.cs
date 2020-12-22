using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Simplify.SeedWork
{
    public interface ICommandDispatcher
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task SendCommand(object command, CancellationToken ct = default);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Task<TCommandResponse> SendCommand<TCommandResponse>(ICommand<TCommandResponse> command, CancellationToken ct = default)
            where TCommandResponse : ICommandResponse;
    }
}