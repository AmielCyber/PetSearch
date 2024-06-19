using System.Net;
using ErrorOr;

namespace PetSearch.API.Tests.Data;

public class PetFinderClientErrorsData: TheoryData<HttpStatusCode, Error>
{
    
    public PetFinderClientErrorsData()
    {
        Add( HttpStatusCode.BadRequest, Errors.Errors.Pets.BadRequest);
        Add( HttpStatusCode.Unauthorized, Errors.Errors.Token.Unauthorized);
        Add( HttpStatusCode.NotFound, Errors.Errors.Pets.NotFound);
        Add( HttpStatusCode.InternalServerError, Errors.Errors.Pets.ServerError);
    }
}