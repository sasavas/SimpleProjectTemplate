using SimpleProjectTemplate.SharedLibrary.Exceptions;

namespace SimpleProjectTemplate.Application.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException() : base(ErrorCodes.NOT_FOUND) { }
    }
}