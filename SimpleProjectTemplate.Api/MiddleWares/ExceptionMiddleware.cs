using System.Diagnostics;
using SimpleProjectTemplate.Api.Extensions;
using SimpleProjectTemplate.SharedLibrary.Exceptions;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;

namespace SimpleProjectTemplate.Api.MiddleWares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionMiddleware(
        ILogger<ExceptionMiddleware> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BaseException e)
        {
            _logger.LogInformation(e, "{errorCode}", e.ErrorCode.CODE);

            await Results.Problem(
                    detail: e.Message,
                    statusCode: StatusCodes.Status400BadRequest,
                    type: e.ErrorCode.CODE)
                .ExecuteAsync(context);
        }
        catch (UniqueConstraintException e)
        {
            _logger.LogInformation(e, "DB Unique constraint exception");

            await Results.Problem(
                detail: _environment.IsDevelopment() ? e.Message : "Duplicate key exception",
                    statusCode: StatusCodes.Status409Conflict,
                    type: ErrorCodes.DUPLICATE_ENTITY.CODE)
                .ExecuteAsync(context);
        }
        catch (DbUpdateException e)
        {
            _logger.LogError(e, "Error while database operation");
            
            await Results.Problem(
                    detail: "Could not finish database operation successfully",
                    statusCode: StatusCodes.Status500InternalServerError,
                    type: ErrorCodes.DB_UPDATE_ERROR.CODE)
                .ExecuteAsync(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not process a request with exception");
            
            await Results.Problem(
                    _environment.IsDevelopment() ? e.WithStackTrace() : "Oops, we made a mistake",
                    "",
                    StatusCodes.Status500InternalServerError,
                    "An Error occurred",
                    ErrorCodes.INTERNAL_ERROR.CODE,
                    new Dictionary<string, object?>()
                    {
                        {"traceId", Activity.Current?.Id}
                    })
                .ExecuteAsync(context);
        }
    }
}