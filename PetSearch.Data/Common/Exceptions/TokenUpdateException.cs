namespace PetSearch.Data.Common.Exceptions;

/// <summary>
/// Exception when we failed to update token in database.
/// </summary>
public class TokenUpdateException : Exception
{
    public TokenUpdateException(string msg = "") : base($"Failed to update token. {msg}")
    {
    }
}