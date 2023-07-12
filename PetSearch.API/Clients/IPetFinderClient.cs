using ErrorOr;
using PetSearch.API.Models.PetFinderResponse;
using PetSearch.API.RequestHelpers;

namespace PetSearch.API.Clients;

/// <summary>
/// Pet Finder Client interface to handle PetFinder API requests.
/// Such as getting a list of pets and a single pet object from the PetFinder API.
/// </summary>
public interface IPetFinderClient
{
    public Task<ErrorOr<PetsResponseDto>> GetPets(PetsParams petsParams);
    public Task<ErrorOr<PetDto>> GetSinglePet(int id);
}