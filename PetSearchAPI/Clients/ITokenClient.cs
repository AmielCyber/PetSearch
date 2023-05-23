using PetSearchAPI.Models.Token;

namespace PetSearchAPI.Clients;

public interface ITokenClient
{
    public Task<TokenResponseDto> GetToken();
}