using System.Collections.Immutable;
using System.Net;

namespace PetSearch.API.Problems;

public class MapBoxProblems : IExpectedProblems
{
    public string ForbiddenErrorMessage => "Forbidden response generated from MapBox API";

    private readonly ImmutableDictionary<HttpStatusCode, string> _expectedErrorCodes =
        new Dictionary<HttpStatusCode, string>()
        {
            { HttpStatusCode.BadRequest, "Invalid parameters entered." },
            { HttpStatusCode.Unauthorized, "MapBox access token is invalid or has expired." },
            { HttpStatusCode.NotFound, "Location was not found. Please enter a valid zip code or coordinates" },
        }.ToImmutableDictionary();


    public string GetProblemDetails(HttpStatusCode statusCode)
    {
        if (_expectedErrorCodes.TryGetValue(statusCode, out var details))
            return details;
        throw new Exception($"Error code: {statusCode} not supported for {nameof(MapBoxProblems)}.");
    }

    public bool IsExpectedErrorCode(HttpStatusCode statusCode) => _expectedErrorCodes.ContainsKey(statusCode);
}