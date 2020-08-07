namespace Simplify.SeedWork.Commands
{
    public interface ICommand { }

    public interface ICommand<out TCommandResponse> : ICommand where TCommandResponse : ICommandResponse { }
}