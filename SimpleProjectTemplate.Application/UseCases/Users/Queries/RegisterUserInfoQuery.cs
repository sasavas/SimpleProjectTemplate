using SimpleProjectTemplate.Domain.Features.Authentication.ValueObjects;
using MediatR;

namespace SimpleProjectTemplate.Application.UseCases.Users.Queries;

public sealed record RegisterUserInfoQuery : IRequest<RegisterUserInfoDto>;

public sealed record RegisterUserInfoDto(
    IEnumerable<string> GenderCodes);


public sealed class RegisterUserInfoQueryHandler
    : IRequestHandler<RegisterUserInfoQuery, RegisterUserInfoDto>
{
    public Task<RegisterUserInfoDto> Handle(RegisterUserInfoQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(
            new RegisterUserInfoDto(
                Gender.AllGenders.Select(gender => gender.Value)));
    }
}