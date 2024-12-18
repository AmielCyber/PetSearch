using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using PetSearch.API.Clients;
using PetSearch.API.Models;
using PetSearch.API.Helpers;

namespace PetSearch.API.Handlers;

public class PetHandler 
{
    /// <summary>
    /// Get a list of available pets for adoption. 
    /// </summary>
    /// <remarks>The pet list is based on the query parameters passed such as the available pets within distance 
    /// of a zip code
    /// </remarks>
    /// <param name="petsParams">Pets parameters</param>
    /// <param name="petFinderClient">Dependency Injection for IPetFinderClient</param>
    /// <param name="httpResponse">Dependency Injection to add response headers</param>
    /// <returns>
    /// Returns a list available pets and the pagination object if successful, else returns a
    /// problem detail.
    /// </returns>
    /// <response code="200">Returns the pet list from the search query and its pagination metadata.</response>
    /// <response code="400">If search parameters contains invalid values.</response>
    public static async Task<Results<Ok<List<PetDto>>, ProblemHttpResult>> GetPetsAsync(
        [AsParameters] PetsParams petsParams,
        IPetFinderClient petFinderClient,
        HttpResponse httpResponse
    )
    {
        Results<Ok<PagedList<PetDto>>, ProblemHttpResult> results = await petFinderClient.GetPetsAsync(petsParams);

        if (results.Result is not Ok<PagedList<PetDto>> petsResult)
            return results.Result as ProblemHttpResult ?? TypedResults.Problem(statusCode: 500);
        
        List<PetDto> petDtoList = petsResult.Value?.Items ?? new List<PetDto>();
        PaginationMetaData paginationMetaData = petsResult.Value?.Pagination ?? new PaginationMetaData(0,0,0,0);
        httpResponse.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetaData));
        httpResponse.Headers.CacheControl = ($"private,max-age={TimeSpan.FromMinutes(10).TotalSeconds}");
        return TypedResults.Ok(petDtoList);
    }

    /// <summary>
    /// Gets a single pet object.
    /// </summary>
    /// <remarks>The pet object contains the pet's attributes and description.</remarks>
    /// <param name="id">The id of the pet object to fetch.</param>
    /// <param name="petFinderClient">Dependency Injection for IPetFinderClient</param>
    /// <returns>A pet object if request was successful, else a problem detail.</returns>
    /// <response code="200">Returns the pet object.</response>
    /// <response code="400">If id is not an integer.</response>
    /// <response code="404">If pet with passed id is not found or has been adopted.</response>
    public static async Task<Results<Ok<PetDto>, ProblemHttpResult>> GetSinglePetAsync(
        int id,
        IPetFinderClient petFinderClient
    )
    {
        return await petFinderClient.GetSinglePetAsync(id);
    }
}