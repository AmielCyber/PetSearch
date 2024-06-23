namespace PetSearch.API.Exceptions;

/// <summary>
/// Exception class created when we can not access to an API endpoint due to a forbidden response.
/// </summary>
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message) : base(message)
    {
    }
}