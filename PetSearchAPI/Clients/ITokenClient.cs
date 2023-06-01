using PetSearchAPI.Models.Token;

namespace PetSearchAPI.Clients;

/// <summary>
/// Pet Finder Token Client interface to fetch a request token from PetFinder.
/// </summary>
public interface ITokenClient
{
    public Task<TokenResponseDto> GetToken();
}