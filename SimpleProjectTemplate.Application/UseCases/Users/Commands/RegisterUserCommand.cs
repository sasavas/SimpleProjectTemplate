using SimpleProjectTemplate.Application.UseCases.Users.Exceptions;
using SimpleProjectTemplate.Domain.Features.Authentication;
using SimpleProjectTemplate.Domain.Features.Authentication.ValueObjects;
using SimpleProjectTemplate.SharedLibrary.AzureServiceBus.EmailQueue;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleProjectTemplate.Application.Ports;
using SimpleProjectTemplate.Domain.DataAccess;
using SimpleProjectTemplate.Domain.Features.Authentication.DataAccess;

namespace SimpleProjectTemplate.Application.UseCases.Users.Commands;

public record RegisterUserCommand(
    string Email,
    string Password,
    string LanguageCode) : IRequest<User>;

public class RegisterUserCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IMessageSenderGateway messageSenderGateway,
    ILogger<RegisterUserCommandHandler> logger) : IRequestHandler<RegisterUserCommand, User>
{
    public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = userRepository.GetByEmail(request.Email);
        if (existingUser is not null)
        {
            throw new UserWithSameEmailAlreadyExistsException();
        }

        var defaultUserRole = roleRepository.GetList().Single(role => role.Name.Equals("User"));
        
        try
        {
            unitOfWork.BeginTransaction();

            // create and save the user
            var verificationCode = Guid.NewGuid();
            
            var user = User.Create(new Email(request.Email),
                                   new Password(request.Password),
                                   defaultUserRole,
                                   verificationCode);
            
            var createdUser = await userRepository.Insert(user);
            
            // Send email to user-to-register
            await messageSenderGateway.SendMessageAsync(
                Constants.EmailQueueName,
                new EmailQueueMessageBody(request.Email,
                    "Welcome to MyApp",
                    $"""
                         <h1>Welcome to MyApp!</h1>
                         <p>Please verify to complete your registration process</p>
                         <a href="http://localhost:4200/lobby/verification/{verificationCode}">Click to Verify</a>
                     """));

            unitOfWork.Commit();

            return createdUser;
        }
        catch (Exception e)
        {
            unitOfWork.Rollback();
            logger.Log(LogLevel.Error, e, "Error occurred while user registration");
            throw;
        }
    }
}