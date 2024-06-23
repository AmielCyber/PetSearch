using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PetSearch.API.Handlers;

/// <summary>
/// Base handler function for getting problem details for every handler that inherits this class.
/// </summary>
public class BaseHandler
{
    /// <summary>
    /// Maps an Error type to a problem details object.
    /// </summary>
    /// <param name="error">Error</param>
    /// <returns>A problem details object(rfc7807 compliant).</returns>
    protected static ProblemHttpResult GetProblemHttpResult(Error error)
    {
        int statusCode = error.NumericType switch
        {
            (int)ErrorType.Validation => StatusCodes.Status400BadRequest,
            (int)ErrorType.NotFound => StatusCodes.Status404NotFound,
            (int)ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        return TypedResults.Problem(statusCode: statusCode, detail: error.Description);
    }
}