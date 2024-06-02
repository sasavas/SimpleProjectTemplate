using SimpleProjectTemplate.Domain.Abstract;

namespace SimpleProjectTemplate.Domain.DataAccess.Contracts;

public interface IMtRepository<TAggregateRoot, in TId> : IRepository<TAggregateRoot, TId> 
    where TAggregateRoot : AggregateRootMt<TId> where TId : notnull
{
    Task<TAggregateRoot> CreateLibraryItem(TAggregateRoot entity);
}