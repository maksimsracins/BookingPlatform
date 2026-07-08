using BookingPlatform.Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace BookingPlatform.Api;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()),
                cancellationToken);

            return true;
        }

        var statusCode = exception switch
        {
            BusinessNotFoundException => StatusCodes.Status404NotFound,
            ClientNotFoundException => StatusCodes.Status404NotFound,
            EmployeeNotFoundException => StatusCodes.Status404NotFound,
            ServiceNotFoundException => StatusCodes.Status404NotFound,
            EmployeeBusyException => StatusCodes.Status409Conflict,

            _ => StatusCodes.Status500InternalServerError
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