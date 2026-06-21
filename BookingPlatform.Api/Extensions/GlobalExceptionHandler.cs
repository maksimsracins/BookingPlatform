
using BookingPlatform.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace BookingPlatform.Api.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
    var statusCode = exception switch
    {
        BusinessNotFoundException => StatusCodes.Status404NotFound,
        ClientNotFoundException => StatusCodes.Status404NotFound,
        EmployeeNotFoundException => StatusCodes.Status404NotFound,
        ServiceNotFoundException => StatusCodes.Status404NotFound,

        EmployeeBusyException => Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict,

        _ => Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
    };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            new
            {
                error = exception.Message
            },
            cancellationToken);

        return true;
    }
}