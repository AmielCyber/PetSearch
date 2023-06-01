using System.Net;
using ErrorOr;
using PetSearchAPI.Common.Errors;

namespace PetSearchAPI.Tests.PetFinderClientTests;

public static class PetFinderTestHelper
{
    public static Error GetExpectedErrorType(HttpStatusCode responseStatusCode)
    {
        return responseStatusCode switch
        {
            HttpStatusCode.BadRequest => Errors.Pets.BadRequest,
            HttpStatusCode.Unauthorized => Errors.Token.NotAuthorized,
            HttpStatusCode.NotFound => Errors.Pets.NotFound,
            _ => Errors.Pets.ServerError,
        };
    }
}