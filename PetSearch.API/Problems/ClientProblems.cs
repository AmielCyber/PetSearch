using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using PetSearch.API.Exceptions;

namespace PetSearch.API.Problems;

public static class ClientProblems
{
    public static ProblemHttpResult GetProblemHttpResult(HttpStatusCode httpStatusCode,
        IExpectedProblems expectedProblems
    )
    {
        ThrowIfResponseIsForbidden(httpStatusCode, expectedProblems);
        return GetProblemFromHttpStatusCode(httpStatusCode, expectedProblems);
    }

    private static ProblemHttpResult GetProblemFromHttpStatusCode(HttpStatusCode httpStatusCode,
        IExpectedProblems expectedProblems
    )
    {
        if (!expectedProblems.IsExpectedErrorCode(httpStatusCode))
            return TypedResults.Problem(detail: "Something went wrong with your request.",
                statusCode: StatusCodes.Status500InternalServerError);

        return TypedResults.Problem(detail: expectedProblems.GetProblemDetails(httpStatusCode),
            statusCode: (int)httpStatusCode);
    }

    private static void ThrowIfResponseIsForbidden(HttpStatusCode httpStatusCode,
        IExpectedProblems expectedProblems)
    {
        if (httpStatusCode == HttpStatusCode.Forbidden)
            throw new ForbiddenAccessException(expectedProblems.ForbiddenErrorMessage);
    }
}