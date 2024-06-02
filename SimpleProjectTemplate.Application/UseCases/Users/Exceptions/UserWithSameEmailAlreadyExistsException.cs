using SimpleProjectTemplate.Application.Exceptions;
using SimpleProjectTemplate.SharedLibrary.Exceptions;

namespace SimpleProjectTemplate.Application.UseCases.Users.Exceptions;

public class UserWithSameEmailAlreadyExistsException : AppException
{
    public UserWithSameEmailAlreadyExistsException() : base(ErrorCodes.USER_EMAIL_TAKEN)
    {
    }
}