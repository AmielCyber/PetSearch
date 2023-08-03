namespace PetSearch.API.Common.Exceptions;

public class MapBoxForbidden: Exception
{
    public MapBoxForbidden() : base("Forbidden response from MapBox API")
    {
    }
}