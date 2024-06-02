using SimpleProjectTemplate.Application.Exceptions;
using SimpleProjectTemplate.Domain.Features.Authentication.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleProjectTemplate.Domain.DataAccess;
using SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;

namespace SimpleProjectTemplate.Application.UseCases.Users.Commands;

public record CompleteOnboardingCommand(
    string? FirstName,
    string? LastName,
    string? Gender,
    DateOnly? DateOfBirth) : IRequest;

public class CompleteOnboardingRequestHandler(
    IUserIdProvider userIdProvider,
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    ILogger<CompleteOnboardingRequestHandler> logger)
    : IRequestHandler<CompleteOnboardingCommand>
{
    public async Task Handle(CompleteOnboardingCommand command, CancellationToken cancellationToken)
    {
        var foundUser = await userRepository.GetById(userIdProvider.GetUserId())
                        ?? throw new NotFoundException();

        if (command.FirstName != null) foundUser.FirstName = command.FirstName;
        if (command.LastName != null) foundUser.LastName = command.LastName;
        if (command.DateOfBirth != null) foundUser.DateOfBirth = command.DateOfBirth;
        if (command.Gender != null) foundUser.Gender = new Gender(command.Gender);
        
        unitOfWork.BeginTransaction();
        try
        {
            userRepository.Update(foundUser);
            unitOfWork.Commit();
        }
        catch (Exception e)
        {
            unitOfWork.Rollback();
            logger.LogError(e, "Error while saving User Onboarding Info");
        }
    }
}