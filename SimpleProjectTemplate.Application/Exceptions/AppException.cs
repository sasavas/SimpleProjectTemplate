using SimpleProjectTemplate.SharedLibrary.Exceptions;

namespace SimpleProjectTemplate.Application.Exceptions;

public class AppException : BaseException
{
    public AppException(ErrorCode errorCode) : base(errorCode)
    {
    }
}