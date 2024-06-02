using SimpleProjectTemplate.Application.Exceptions;
using SimpleProjectTemplate.SharedLibrary.Exceptions;

namespace SimpleProjectTemplate.Application.UseCases.Users.Exceptions;

public class UserNotVerifiedException : AppException
{
    public UserNotVerifiedException() : base(ErrorCodes.USER_NOT_VERIFIED)
    {
    }
}