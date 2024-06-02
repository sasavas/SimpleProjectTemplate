namespace SimpleProjectTemplate.Domain.Abstract;

/// <summary>
/// This is the root for each feature.
/// The data can be written to and is retrieved from the Database only
/// by the Aggregate Root for consistency.
/// </summary>
/// <typeparam name="TId"></typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId: notnull
{
    protected AggregateRoot(TId id) : base(id)
    {
        
    }
    
    protected AggregateRoot(){}
}