using System.Collections.Generic;

namespace Simplify.Core.Domain
{
    public interface IDomainEvents
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    }
}