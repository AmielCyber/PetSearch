namespace PetSearch.Data.Common.Exceptions;

public class TokenUpdateException : Exception
{
    public TokenUpdateException(string msg = "") : base($"Failed to update token. {msg}")
    {
    }
}