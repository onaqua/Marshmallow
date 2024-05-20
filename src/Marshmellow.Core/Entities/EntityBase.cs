using MediatR;

namespace Marshmallow.Core.Entities;

public abstract class EntityBase 
{
    private HashSet<INotification> _domainEvents = new HashSet<INotification>();

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents;

    public void AddDomainEvent(INotification notification)
    {
        _domainEvents.Add(notification);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public abstract class DomainEvent : INotification
{ 
}

public abstract class EntityBase<TKey> : EntityBase
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; protected set; } = default!;
}

public abstract class ValueObject
{ 

}