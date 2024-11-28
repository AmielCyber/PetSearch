using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using PetSearch.API.Exceptions;

namespace PetSearch.API.Helpers;

public static class MapBoxProblems
{
    private static readonly List<HttpStatusCode> ExpectedErrorStatusCodes =
    [
        HttpStatusCode.BadRequest,
        HttpStatusCode.Unauthorized,
        HttpStatusCode.NotFound,
    ];
    
    public static ProblemHttpResult GetProblemFromZipcode(HttpResponseMessage httpResponseMessage, string zipcode)
    {
        ThrowIfResponseIsForbidden(httpResponseMessage);
        string? detail = null;
        if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            detail = $"The zipcode entered ({zipcode}) does not exist.";
        return GetProblemFromHttpStatusCode(httpResponseMessage.StatusCode, detail);
    }

    public static ProblemHttpResult GetProblemFromCoords(HttpResponseMessage httpResponseMessage, double longitude, double latitude)
    {
        ThrowIfResponseIsForbidden(httpResponseMessage);
        string? detail = null;
        if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            detail = "The coordinates entered does not exist or are invalid.";
        return GetProblemFromHttpStatusCode(httpResponseMessage.StatusCode, detail);
    }

    private static ProblemHttpResult GetProblemFromHttpStatusCode(HttpStatusCode httpStatusCode, string? detail)
    {
        if (!IsExpectedStatusCode(httpStatusCode))
            return TypedResults.Problem(statusCode: StatusCodes.Status500InternalServerError);
        return TypedResults.Problem(detail: detail, statusCode: (int)httpStatusCode);
    }

    private static bool IsExpectedStatusCode(HttpStatusCode statusCode) => ExpectedErrorStatusCodes.Contains(statusCode);

    private static void ThrowIfResponseIsForbidden(HttpResponseMessage httpResponseMessage)
    {
        if(httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
            throw new ForbiddenAccessException("Forbidden response generated from MapBox API");
    }
}