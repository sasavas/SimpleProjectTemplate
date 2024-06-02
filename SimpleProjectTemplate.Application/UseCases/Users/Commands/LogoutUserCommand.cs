using SimpleProjectTemplate.Domain.Features.Authentication;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleProjectTemplate.Domain.DataAccess;

namespace SimpleProjectTemplate.Application.UseCases.Users.Commands;

public sealed record LogoutUserCommand(string Token, DateTime ActualExpiryDate) : IRequest;

public sealed class LogoutUserCommandHandler(
    ILogger<LogoutUserCommandHandler> logger,
    IUnitOfWork unitOfWork,
    IBlacklistedTokenRepository blacklistedTokenRepository) 
    : IRequestHandler<LogoutUserCommand>
{
    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        unitOfWork.BeginTransaction();

        try
        {
            blacklistedTokenRepository.Insert(BlacklistedToken.Create(request.Token, DateTime.UtcNow, request.ActualExpiryDate.ToUniversalTime()));
            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not log out user");
            unitOfWork.Rollback();
        }
    }
}