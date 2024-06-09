namespace PetSearch.API.Common.Exceptions;

/// <summary>
/// Exception object when we obtain a forbidden response from MapBox API.
/// </summary>
public class MapBoxForbidden : Exception
{
    public MapBoxForbidden() : base("Forbidden response from MapBox API")
    {
    }
}