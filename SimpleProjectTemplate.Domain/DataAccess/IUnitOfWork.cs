namespace SimpleProjectTemplate.Domain.DataAccess;

public interface IUnitOfWork : IDisposable
{
    void SaveChanges();
    void BeginTransaction();
    void Commit();
    void Rollback();
}