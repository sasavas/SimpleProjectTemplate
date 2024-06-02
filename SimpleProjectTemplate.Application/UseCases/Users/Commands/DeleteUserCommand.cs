using MediatR;
using Microsoft.Extensions.Logging;
using SimpleProjectTemplate.Domain.DataAccess;
using SimpleProjectTemplate.Domain.Features.Authentication;
using SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;

namespace SimpleProjectTemplate.Application.UseCases.Users.Commands;

public sealed record DeleteUserCommand(Guid UserId) : IRequest;

public class DeleteUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    ILogger<DeleteUserCommandHandler> logger) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        unitOfWork.BeginTransaction();

        try
        {
            userRepository.Delete(request.UserId);
            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Could not delete User {userId}", request.UserId);
            throw;
        }
    }
}