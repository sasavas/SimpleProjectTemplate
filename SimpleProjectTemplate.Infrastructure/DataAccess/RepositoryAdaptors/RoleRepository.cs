using SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;
using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;
using SimpleProjectTemplate.Infrastructure.DataAccess.Repositories;

namespace SimpleProjectTemplate.Infrastructure.DataAccess.RepositoryAdaptors;

internal class RoleRepository : SimpleRepositoryDecorator<Role, int>, IRoleRepository
{
    public RoleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}