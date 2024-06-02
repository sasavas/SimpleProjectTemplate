using SimpleProjectTemplate.Domain.DataAccess.Contracts;
using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;

namespace SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;

public interface IRoleRepository : IRepository<Role, int>
{
    
}