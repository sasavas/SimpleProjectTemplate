using SimpleProjectTemplate.Domain.Features.Authentication.RoleModule;
using MediatR;
using SimpleProjectTemplate.Domain.Features.Authentication;
using SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;

namespace SimpleProjectTemplate.Application.UseCases.Users.Queries;

public record UserPermissionQuery(Guid UserId) : IRequest<IEnumerable<Permission>?>;

public class UserPermissionQueryHandler : IRequestHandler<UserPermissionQuery, IEnumerable<Permission>?>
{
    private readonly IUserRepository _userRepository;

    public UserPermissionQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<IEnumerable<Permission>?> Handle(UserPermissionQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userRepository.GetUserPermissions(query.UserId));
    }
}