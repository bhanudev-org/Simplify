namespace Simplify.Core.Domain
{
    public interface IHandler<in TEvent> where TEvent : IDomainEvent
    {
        void Handle(TEvent domainEvent);
    }
}