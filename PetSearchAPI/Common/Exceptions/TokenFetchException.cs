namespace PetSearchAPI.Common.Exceptions;

/// <summary>
/// Exception when the server failed to obtain a token from PetFinder API.
/// </summary>
public class TokenFetchException : Exception
{
    public TokenFetchException() : base("Failed to fetch token from our server.")
    {
    }
}