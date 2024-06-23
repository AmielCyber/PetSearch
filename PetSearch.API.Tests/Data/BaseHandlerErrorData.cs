using System.Net;
using ErrorOr;

namespace PetSearch.API.Tests.Data;

public class BaseHandlerErrorData: TheoryData<Error, HttpStatusCode>
{
    public BaseHandlerErrorData()
    {
        Add(Error.Validation(), HttpStatusCode.BadRequest);
        Add(Error.NotFound(), HttpStatusCode.NotFound);
        Add(Error.Unauthorized(), HttpStatusCode.Unauthorized);
        Add(Error.Failure(), HttpStatusCode.InternalServerError);
    }
}