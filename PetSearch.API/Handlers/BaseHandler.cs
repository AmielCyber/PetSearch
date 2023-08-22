using ErrorOr;
using PetSearch.API.Common.Errors;

namespace PetSearch.API.Handlers;

/// <summary>
/// Base handler function for getting problem details for every handler that inherits this class.
/// </summary>
public class BaseHandler
{
    /// <summary>
    /// Returns a problem detail that is rfc7807 compliant based on the list of custom Errors
    /// that occurred while using the PetFinder API (https://tools.ietf.org/html/rfc7807).
    /// </summary>
    /// <param name="errors">List of custom ErrorOr Errors</param>
    /// <returns>A problem details object</returns>
    protected static IResult GetProblems(List<Error> errors)
    {
        var firstError = errors[0];

        var statusCode = firstError.NumericType switch
        {
            (int)ErrorType.Validation => StatusCodes.Status400BadRequest,
            (int)ErrorType.NotFound => StatusCodes.Status404NotFound,
            MyErrorTypes.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Results.Problem(statusCode: statusCode, detail: firstError.Description);
    }
}