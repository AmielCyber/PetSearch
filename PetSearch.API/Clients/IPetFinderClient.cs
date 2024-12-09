using Microsoft.AspNetCore.Http.HttpResults;
using PetSearch.API.Models;
using PetSearch.API.Helpers;

namespace PetSearch.API.Clients;

/// <summary>
/// Pet Finder Client interface to handle PetFinder API requests,
/// such as getting a list of pets and a single pet object from the PetFinder API.
/// </summary>
public interface IPetFinderClient
{
    public Task<Results<Ok<PagedList<PetDto>>, ProblemHttpResult>> GetPetsAsync(PetsParams petsParams);
    public Task<Results<Ok<PetDto>, ProblemHttpResult>> GetSinglePetAsync(int id);
}