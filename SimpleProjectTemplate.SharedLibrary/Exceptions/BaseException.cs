namespace SimpleProjectTemplate.SharedLibrary.Exceptions;

public abstract class BaseException : Exception
{
    public readonly ErrorCode ErrorCode;

    protected BaseException(ErrorCode errorCode) : base(errorCode.DESCRIPTION)
    {
        ErrorCode = errorCode;
    }
}