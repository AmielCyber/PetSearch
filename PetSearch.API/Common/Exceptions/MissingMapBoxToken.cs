namespace PetSearch.API.Common.Exceptions;

public class MissingMapBoxToken: Exception
{
    public MissingMapBoxToken(): base("Missing MapBox Token. User secrets for MapBox may not been set up.")
    {
        
    }
    
}