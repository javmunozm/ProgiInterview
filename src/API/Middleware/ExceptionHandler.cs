using System.Net;
using FluentValidation;

namespace API.Middleware;

public static class ExceptionHandler
{
    public static (HttpStatusCode StatusCode, IEnumerable<ErrorDetail> Errors) Handle(Exception exception) =>
        exception switch
        {
            ValidationException ex => (HttpStatusCode.BadRequest,
                ex.Errors.Select(e => new ErrorDetail(e.PropertyName, e.ErrorMessage))),

            ArgumentException ex => (HttpStatusCode.BadRequest,
                [new ErrorDetail(string.Empty, ex.Message)]),

            _ => (HttpStatusCode.InternalServerError,
                [new ErrorDetail(string.Empty, "An unexpected error occurred.")])
        };
}

public record ErrorDetail(string PropertyName, string ErrorMessage);
