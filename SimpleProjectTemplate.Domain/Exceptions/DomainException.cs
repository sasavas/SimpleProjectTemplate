using SimpleProjectTemplate.SharedLibrary.Exceptions;

namespace SimpleProjectTemplate.Domain.Exceptions;

public class DomainException : BaseException
{
    protected DomainException(ErrorCode error) : base(error)
    {
    }
}