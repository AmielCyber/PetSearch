using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using PetSearchAPI.Common.Errors;

namespace PetSearchAPI.Controllers;

/// <summary>
/// Base controller for all controller in our API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    /// <summary>
    /// Returns a problem detail that is rfc7807 compliant based on the list of custom Errors
    /// that occurred while using the PetFinder API (https://tools.ietf.org/html/rfc7807).
    /// </summary>
    /// <param name="errors">List of custom ErrorOr Errors</param>
    /// <returns>A problem details object</returns>
    [NonAction]
    protected IActionResult GetProblems(List<Error> errors)
    {
        var firstError = errors[0];

        var statusCode = firstError.NumericType switch
        {
            (int)ErrorType.Validation => StatusCodes.Status400BadRequest,
            (int)ErrorType.NotFound => StatusCodes.Status404NotFound,
            MyErrorTypes.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}