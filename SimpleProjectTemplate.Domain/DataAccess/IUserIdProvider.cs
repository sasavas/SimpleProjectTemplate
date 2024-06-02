namespace SimpleProjectTemplate.Domain.DataAccess;

public interface IUserIdProvider
{
    Guid GetUserId();
}