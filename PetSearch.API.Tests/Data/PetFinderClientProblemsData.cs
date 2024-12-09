using System.Net;

namespace PetSearch.API.Tests.Data;

public class PetFinderClientProblemsData: TheoryData<HttpStatusCode>
{
    public PetFinderClientProblemsData()
    {
        Add(HttpStatusCode.BadRequest);
        Add(HttpStatusCode.Unauthorized);
        Add(HttpStatusCode.NotFound);
        Add(HttpStatusCode.InternalServerError);
    }
    
}