using System.Net;

namespace PetSearch.API.Tests.Data;

public class MapBoxClientProblemsData: TheoryData<HttpStatusCode>
{
    public MapBoxClientProblemsData()
    {
        Add( HttpStatusCode.BadRequest);
        Add( HttpStatusCode.Unauthorized);
        Add( HttpStatusCode.NotFound);
        Add( HttpStatusCode.InternalServerError);
    }
    
}