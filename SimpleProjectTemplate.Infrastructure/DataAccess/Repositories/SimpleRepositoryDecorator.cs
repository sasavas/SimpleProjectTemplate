using System.Linq.Expressions;
using SimpleProjectTemplate.Domain.Abstract;
using SimpleProjectTemplate.Domain.DataAccess.Contracts;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.Repositories;

public class SimpleRepositoryDecorator<TEntity, TId> : IRepository<TEntity, TId> 
    where TEntity : AggregateRoot<TId> where TId : notnull
{
    protected readonly BaseRepositoryImpl<TEntity, TId> BaseRepositoryImpl;

    internal SimpleRepositoryDecorator(AppDbContext dbContext)
    {
        BaseRepositoryImpl = new BaseRepositoryImpl<TEntity, TId>(dbContext);
    }
    
    public async Task<TEntity> Insert(TEntity entity)
    {
        return await BaseRepositoryImpl.Create(entity);
    }

    public TEntity Update(TEntity entity)
    {
        return BaseRepositoryImpl.Update(entity);
    }

    public async Task Delete(TId id)
    {
        await BaseRepositoryImpl.Delete(id);
    }

    public async Task<TEntity?> GetById(TId id)
    {
        return await BaseRepositoryImpl.GetById(id);
    }

    public IEnumerable<TEntity> GetList()
    {
        return BaseRepositoryImpl.GetList();
    }

    public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
    {
        return BaseRepositoryImpl.GetList(filter);
    }
}