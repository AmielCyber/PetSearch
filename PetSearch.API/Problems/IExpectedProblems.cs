using System.Net;

namespace PetSearch.API.Problems;

public interface IExpectedProblems
{
    public string ForbiddenErrorMessage { get; }
    public string GetProblemDetails(HttpStatusCode statusCode);
    public bool IsExpectedErrorCode(HttpStatusCode statusCode);
}