using System.Net;
using ErrorOr;
using PetSearch.API.Common.Errors;

namespace PetSearch.API.Tests.PetFinderClientTests;

public static class PetFinderTestHelper
{
    public static Error GetExpectedErrorType(HttpStatusCode responseStatusCode)
    {
        return responseStatusCode switch
        {
            HttpStatusCode.BadRequest => Errors.Pets.BadRequest,
            HttpStatusCode.Unauthorized => Errors.Token.Unauthorized,
            HttpStatusCode.NotFound => Errors.Pets.NotFound,
            _ => Errors.Pets.ServerError,
        };
    }
}