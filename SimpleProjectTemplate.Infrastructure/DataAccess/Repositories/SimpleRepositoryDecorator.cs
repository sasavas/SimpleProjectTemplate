using System.Linq.Expressions;
using SimpleProjectTemplate.Domain.Abstract;
using SimpleProjectTemplate.Domain.DataAccess.Contracts;
using SimpleProjectTemplate.Infrastructure.DataAccess.Pagination;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.Repositories;

public class SimpleRepositoryDecorator<TEntity, TId> : IRepository<TEntity, TId> 
    where TEntity : AggregateRoot<TId> where TId : notnull
{
    protected readonly BaseRepositoryImpl<TEntity, TId> BaseRepositoryImpl;

    internal SimpleRepositoryDecorator(AppDbContext dbContext)
    {
        BaseRepositoryImpl = new BaseRepositoryImpl<TEntity, TId>(dbContext);
    }
    
    public virtual async Task<TEntity> Insert(TEntity entity)
    {
        return await BaseRepositoryImpl.Create(entity);
    }

    public virtual TEntity Update(TEntity entity)
    {
        return BaseRepositoryImpl.Update(entity);
    }

    public virtual async Task Delete(TId id)
    {
        await BaseRepositoryImpl.Delete(id);
    }

    public virtual async Task<TEntity?> GetById(TId id)
    {
        return await BaseRepositoryImpl.GetById(id);
    }

    public virtual IEnumerable<TEntity> GetList()
    {
        return BaseRepositoryImpl.GetQueryable().ToList();
    }

    public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
    {
        return BaseRepositoryImpl.GetQueryable().Where(filter).ToList();
    }
    
    public virtual async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPaginatedListAsync(
        Expression<Func<TEntity, bool>> filter,
        PaginationParams paginationParams)
    {
        return await BaseRepositoryImpl.GetPaginatedListAsync(filter, paginationParams);
    }

    // Expose IQueryable for more complex queries
    protected virtual IQueryable<TEntity> Query()
    {
        return BaseRepositoryImpl.GetQueryable();
    }
}