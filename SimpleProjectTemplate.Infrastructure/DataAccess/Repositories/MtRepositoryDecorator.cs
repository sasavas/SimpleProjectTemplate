using System.Linq.Expressions;
using SimpleProjectTemplate.Domain.Abstract;
using SimpleProjectTemplate.Domain.DataAccess;
using SimpleProjectTemplate.Domain.DataAccess.Contracts;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.Repositories;

public class MtRepositoryDecorator<TEntity, TId> : IMtRepository<TEntity, TId>
    where TEntity : AggregateRootMt<TId> where TId : notnull
{
    protected readonly BaseRepositoryImpl<TEntity, TId> BaseRepositoryImpl;
    protected readonly Guid _userId;

    internal MtRepositoryDecorator(
        IUserIdProvider userIdProvider,
        AppDbContext dbContext)
    {
        _userId = userIdProvider.GetUserId();
        BaseRepositoryImpl = new BaseRepositoryImpl<TEntity, TId>(dbContext);
    }

    public virtual Task<TEntity> Insert(TEntity entity)
    {
        entity.UserId = _userId;
        return BaseRepositoryImpl.Create(entity);
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
        return BaseRepositoryImpl.GetQueryable()
            .Where(e => e.UserId == _userId).ToList();
    }

    public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
    {
        return BaseRepositoryImpl.GetQueryable()
            .Where(e => e.UserId == _userId)
            .Where(filter)
            .ToList();
    }

    // Expose IQueryable for more complex queries
    protected virtual IQueryable<TEntity> Query()
    {
        return BaseRepositoryImpl.GetQueryable()
            .Where(e => e.UserId == _userId);
    }
    
    public virtual async Task<TEntity> CreateTenantFreeItem(TEntity entity)
    {
        entity.UserId = default; // not belonging to a specific tenant, hence the TENANT-LESS item
        var inserted = await BaseRepositoryImpl.Create(entity);
        return inserted;
    }
}