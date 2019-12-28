using System;

namespace Simplify.Core.Domain
{
    public interface IEvent
    {
        DateTime EventOccurredOn { get; }

        object EventSource { get; set; }
    }

    public interface ICommand
    {
    }


}