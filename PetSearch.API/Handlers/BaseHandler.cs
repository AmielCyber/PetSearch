using ErrorOr;
using PetSearch.API.Common.Errors;

namespace PetSearch.API.Handlers;

/// <summary>
/// Base handler function for getting problem details for every handler that inherits this class.
/// </summary>
public class BaseHandler
{
    /// <summary>
    /// Maps a list Error type to a problem details object.
    /// </summary>
    /// <param name="errors">List of custom ErrorOr Errors</param>
    /// <returns>A problem details object(rfc7807 compliant).</returns>
    protected static IResult GetProblems(List<Error> errors)
    {
        var firstError = errors.First();

        var statusCode = firstError.NumericType switch
        {
            (int)ErrorType.Validation => StatusCodes.Status400BadRequest,
            (int)ErrorType.NotFound => StatusCodes.Status404NotFound,
            (int)ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Results.Problem(statusCode: statusCode, detail: firstError.Description);
    }
}