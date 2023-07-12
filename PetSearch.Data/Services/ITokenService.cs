using PetSearch.Data.Entity;

namespace PetSearch.Data.Services;

public interface ITokenService
{
    public Task<Token> GetToken();
}