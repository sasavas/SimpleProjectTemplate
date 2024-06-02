using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SimpleProjectTemplate.Domain.Abstract;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.Repositories;

public class BaseRepositoryImpl<TEntity, TId> where TEntity : AggregateRoot<TId> where TId : notnull
{
    private readonly AppDbContext Context;

    internal BaseRepositoryImpl(AppDbContext context)
    {
        Context = context;
    }

    public virtual async Task<TEntity?> GetById(TId id)
    {
        var found = await Context.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (found is ISoftDeletable softDeletable && softDeletable.IsDeleted)
        {
            return null;
        }

        return found;
    }

    public virtual IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> filter)
    {
        var result = Context.Set<TEntity>().Where(filter);
        return typeof(TEntity).IsAssignableFrom(typeof(ISoftDeletable)) 
            ? result.Where(s => !((ISoftDeletable)s).IsDeleted) 
            : result;
    }

    public virtual IQueryable<TEntity> GetList()
    {
        var result = Context.Set<TEntity>();
        return typeof(TEntity).IsAssignableFrom(typeof(ISoftDeletable)) 
            ? result.Where(s => !((ISoftDeletable)s).IsDeleted) 
            : result;
    }

    public virtual async Task<TEntity> Create(TEntity aggregateRoot)
    {
        if (aggregateRoot is IAuditable auditable)
        {
            auditable.CreatedAt = DateTime.UtcNow;
        }
        var inserted = await Context.Set<TEntity>().AddAsync(aggregateRoot);
        return inserted.Entity;
    }

    public virtual TEntity Update(TEntity entity)
    {
        if (entity is IAuditable auditable)
        {
            auditable.UpdatedAt = DateTime.UtcNow;
        }
        var updated = Context.Set<TEntity>().Update(entity);
        return updated.Entity;
    }

    public virtual async Task Delete(TId entityId)
    {
        var toDelete = await Context.Set<TEntity>().FirstOrDefaultAsync(t => entityId.Equals(t.Id));
        if (toDelete is null) return;

        if (toDelete is ISoftDeletable softDeletable)
        {
            softDeletable.IsDeleted = true;
            softDeletable.DeletedAt = DateTime.UtcNow;
            Context.Set<TEntity>().Update(toDelete);    
        }
        else
        {
            Context.Set<TEntity>().Remove(toDelete);
        }
    }
}