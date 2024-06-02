namespace SimpleProjectTemplate.Api.Extensions;

public static class ExceptionExtension
{
    public static string WithStackTrace(this Exception exception)
    {
        return $"{exception.Message}-{exception.InnerException?.Message}{Environment.NewLine}{exception.StackTrace}";
    }

    public static string WithStackTrace(this Exception exception, string message)
    {
        return $"{message}{exception.WithStackTrace()}";
    }
}