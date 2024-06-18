using System.Net;
using ErrorOr;

namespace PetSearch.API.Tests.Data;

public class MapBoxClientErrorsData: TheoryData<HttpStatusCode, Error>
{
    public MapBoxClientErrorsData()
    {
        Add( HttpStatusCode.BadRequest, Errors.Errors.Location.BadRequest);
        Add( HttpStatusCode.Unauthorized, Errors.Errors.Token.Unauthorized);
        Add( HttpStatusCode.NotFound, Errors.Errors.Location.NotFound);
        Add( HttpStatusCode.InternalServerError, Errors.Errors.Location.ServerError);
    }
}