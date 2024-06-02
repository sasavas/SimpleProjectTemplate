using SimpleProjectTemplate.Domain.Exceptions;
using SimpleProjectTemplate.SharedLibrary.Exceptions;

namespace SimpleProjectTemplate.Domain.Features.Authentication.Exceptions;

public class GenderValidationException : ValidationException
{
    public GenderValidationException() : base(ErrorCodes.USER_NOT_VALID_GENDER)
    {
    }
}