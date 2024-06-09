using PetSearch.Data.Entities;

namespace PetSearch.Data.Services;

/// <summary>
/// ITokenService for storing and retrieving token from our DB.
/// </summary>
public interface ITokenService
{
    public Task<Token> GetToken();
}