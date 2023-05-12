using PetSearchAPI.Models.PetFinderResponse;
using PetSearchAPI.Models.Token;
using PetSearchAPI.RequestHelpers;

namespace PetSearchAPI.Clients;
public interface IPetFinderClient
{
    public Task<TokenResponseDto> GetToken();
    public Task<PetsResponseDto> GetPets(PetsParams petsParams, string token);
    public Task<PetDto> GetSinglePet(int id, string token);
}