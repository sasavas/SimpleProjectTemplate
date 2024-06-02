using SimpleProjectTemplate.SharedLibrary.Exceptions;

namespace SimpleProjectTemplate.Domain.Abstract.Exceptions;

public class ValidationException : BaseException
{
    protected ValidationException(ErrorCode error) : base(error)
    {
    }
}