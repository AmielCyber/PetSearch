namespace PetSearchAPI.Common.Exceptions;

/// <summary>
/// Exception in case we get forbidden error from PetFinder Api.
/// </summary>
public class PetFinderForbidden: Exception
{
    public PetFinderForbidden(): base("Forbidden response from PetFinder API")
    {
        
    }
    
}