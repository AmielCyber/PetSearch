using System.Collections.Immutable;
using System.Net;

namespace PetSearch.API.Problems;

public class PetFinderProblems : IExpectedProblems
{
    public string ForbiddenErrorMessage => "Forbidden response generated from PetFinder API";

    private readonly ImmutableDictionary<HttpStatusCode, string> _expectedErrorCodes =
        new Dictionary<HttpStatusCode, string>()
        {
            { HttpStatusCode.BadRequest, "Invalid parameters entered." },
            { HttpStatusCode.Unauthorized, "PetFinder access token is invalid or has expired." },
            { HttpStatusCode.NotFound, "Pet with the given id doesn't exist." },
        }.ToImmutableDictionary();


    public string GetProblemDetails(HttpStatusCode statusCode)
    {
        if (_expectedErrorCodes.TryGetValue(statusCode, out var details))
            return details;
        throw new Exception($"Error code: {statusCode} not supported for {nameof(PetFinderProblems)}.");
    }

    public bool IsExpectedErrorCode(HttpStatusCode statusCode) => _expectedErrorCodes.ContainsKey(statusCode);
}