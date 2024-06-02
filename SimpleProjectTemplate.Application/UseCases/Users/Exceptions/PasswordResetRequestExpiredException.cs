using SimpleProjectTemplate.Application.Exceptions;
using SimpleProjectTemplate.SharedLibrary.Exceptions;

namespace SimpleProjectTemplate.Application.UseCases.Users.Exceptions;

public class PasswordResetRequestExpiredException : AppException
{
    public PasswordResetRequestExpiredException() 
        : base(ErrorCodes.USER_PASSWORD_RESET_REQUEST_EXPIRED)
    {
    }
}