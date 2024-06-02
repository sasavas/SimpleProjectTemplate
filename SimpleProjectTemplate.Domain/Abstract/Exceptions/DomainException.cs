using SimpleProjectTemplate.SharedLibrary.Exceptions;

namespace SimpleProjectTemplate.Domain.Abstract.Exceptions;

public class DomainException : BaseException
{
    protected DomainException(ErrorCode error) : base(error)
    {
    }
}