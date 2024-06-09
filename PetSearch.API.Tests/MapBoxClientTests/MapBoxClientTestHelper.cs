using System.Net;
using ErrorOr;
using PetSearch.API.Common.Errors;

namespace PetSearch.API.Tests.MapBoxClientTests;

public static class MapBoxClientTestHelper
{
    public static Error GetExpectedErrorType(HttpStatusCode responseStatusCode)
    {
        return responseStatusCode switch
        {
            HttpStatusCode.BadRequest => Errors.Location.BadRequest,
            HttpStatusCode.Unauthorized => Errors.Token.Unauthorized,
            HttpStatusCode.NotFound => Errors.Location.NotFound,
            _ => Errors.Location.ServerError,
        };
    }
}