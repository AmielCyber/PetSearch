using ErrorOr;
using PetSearchAPI.Models.PetFinderResponse;
using PetSearchAPI.Models.Token;
using PetSearchAPI.RequestHelpers;

namespace PetSearchAPI.Clients;

/// <summary>
/// Pet Finder Client interface to handle PetFinder API requests.
/// </summary>
public interface IPetFinderClient
{
    public Task<ErrorOr<PetsResponseDto>> GetPets(PetsParams petsParams, string accessToken);
    public Task<ErrorOr<PetDto>> GetSinglePet(int id, string accessToken);
}