namespace SimpleProjectTemplate.Domain.DomainEvents;

public abstract class DomainEvent
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}