using SimpleProjectTemplate.Domain.DataAccess.Contracts;
using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;

namespace SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;

public interface IUserRepository : IRepository<User, Guid>
{
    User? GetByEmail(string email);

    User? GetByEmailAndPassword(string email, string password);
    
    IEnumerable<Permission>? GetUserPermissions(Guid userId);

    Role GetUserRole(Guid userId);

    User? GetByVerificationCode(string guid);
    
    User? GetUserWithPasswordRequestCode(Guid code);
}