using SimpleProjectTemplate.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleProjectTemplate.Domain.DataAccess;
using SimpleProjectTemplate.Domain.Features.Authentication;
using SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;

namespace SimpleProjectTemplate.Application.UseCases.Users.Commands;

public record VerifyUserCommand(string verificationCode) : IRequest<bool>;

public class VerifyUserCommandHandler(
    IUnitOfWork unitOfWork, IUserRepository userRepository, ILogger<VerifyUserCommandHandler> logger) 
    : IRequestHandler<VerifyUserCommand, bool>
{
    public Task<bool> Handle(VerifyUserCommand request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetByVerificationCode(request.verificationCode);
        if (user is null)
        {
            throw new NotFoundException();
        }

        try
        {
            unitOfWork.BeginTransaction();
            user.IsVerified = true;
            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not verify user");
            throw;
        }
        
        return Task.FromResult(true);
    }
}