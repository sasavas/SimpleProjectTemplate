using SimpleProjectTemplate.Application.Exceptions;
using SimpleProjectTemplate.Domain.Features.Authentication;
using MediatR;
using SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;

namespace SimpleProjectTemplate.Application.UseCases.Users.Queries;

public record LoginQuery(string Email, string Password)
    : IRequest<User>;

public sealed class LoginQueryHandler
    : IRequestHandler<LoginQuery, User>
{
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetByEmailAndPassword(
            query.Email, 
            query.Password);
        
        if (user is null)
            throw new NotFoundException();

        //TODO:production
        // if (user.IsVerified == false)
        //     throw new UserNotVerifiedException();

        return Task.FromResult(user);
    }
}