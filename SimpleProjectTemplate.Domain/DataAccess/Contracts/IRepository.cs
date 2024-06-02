using System.Linq.Expressions;
using SimpleProjectTemplate.Domain.Abstract;

namespace SimpleProjectTemplate.Domain.DataAccess.Contracts;

public interface IRepository<TAggregateRoot, in TId> 
    where TAggregateRoot : AggregateRoot<TId>
    where TId : notnull
{
    Task<TAggregateRoot?> GetById(TId id);

    IEnumerable<TAggregateRoot> GetList();
    
    IEnumerable<TAggregateRoot> GetList(Expression<Func<TAggregateRoot, bool>> filter);

    Task<TAggregateRoot> Insert(TAggregateRoot entity);

    TAggregateRoot Update(TAggregateRoot entity);

    Task Delete(TId entityId);
}