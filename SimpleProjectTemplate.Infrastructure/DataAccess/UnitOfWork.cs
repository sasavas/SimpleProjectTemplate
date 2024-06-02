using SimpleProjectTemplate.Domain.DataAccess;

namespace SimpleProjectTemplate.Infrastructure.DataAccess;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public void SaveChanges()
    {
        context.SaveChanges();
    }

    public void BeginTransaction()
    {
        context.Database.BeginTransaction();
    }

    public void Commit()
    {
        SaveChanges();
        context.Database.CommitTransaction();
    }

    public void Rollback()
    {
        context.Database.RollbackTransaction();
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}
