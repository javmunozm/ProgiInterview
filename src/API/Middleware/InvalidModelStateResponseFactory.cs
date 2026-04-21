using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

public static class InvalidModelStateResponseFactory
{
    public static IActionResult Build(ActionContext context)
    {
        var errors = context.ModelState
            .Where(kvp => kvp.Value is { Errors.Count: > 0 })
            .SelectMany(kvp => kvp.Value!.Errors.Select(err =>
                new ErrorDetail(kvp.Key, SelectMessage(err, kvp.Key))))
            .ToArray();

        return new BadRequestObjectResult(new { errors });
    }

    private static string SelectMessage(Microsoft.AspNetCore.Mvc.ModelBinding.ModelError error, string propertyName)
    {
        if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
            return error.ErrorMessage;

        return $"The value supplied for '{propertyName}' is not valid.";
    }
}
