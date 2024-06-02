using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;
using MediatR;
using SimpleProjectTemplate.Domain.Features.Authentication;
using SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;

namespace SimpleProjectTemplate.Application.UseCases.Users.Queries;

public sealed record UserRoleQuery(Guid UserId) : IRequest<Role>;

public sealed class UserRoleQueryHandler(IUserRepository userRepository) 
    : IRequestHandler<UserRoleQuery, Role>
{
    public Task<Role> Handle(UserRoleQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(userRepository.GetUserRole(query.UserId));
    }
}