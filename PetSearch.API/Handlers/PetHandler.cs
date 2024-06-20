using System.Net.Mime;
using System.Text.Json;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetSearch.API.Clients;
using PetSearch.API.Models;
using PetSearch.API.Helpers;

namespace PetSearch.API.Handlers;

public class PetHandler : BaseHandler
{
    /// <summary>
    /// Get a list of available pets for adoption. 
    /// </summary>
    /// <remarks>The pet list is based on the query parameters passed such as the available pets within distance 
    /// of a zip code
    /// </remarks>
    /// <param name="petsParams">Pets parameters</param>
    /// <param name="petFinderClient">Dependency Injection for IPetFinderClient</param>
    /// <param name="httpContext">Dependency Injection to add add response headers</param>
    /// <returns>
    /// Returns a list available pets and the pagination object if successful, else returns a
    /// problem detail.
    /// </returns>
    /// <response code="200">Returns the pet list from the search query and its pagination metadata.</response>
    /// <response code="400">If search parameters contains invalid values.</response>
    [ProducesResponseType<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest,
        MediaTypeNames.Application.ProblemJson)]
    [Tags("pets")]
    public static async Task<Results<Ok<List<PetDto>>, ProblemHttpResult>> GetPetsAsync(
        [AsParameters] PetsParams petsParams,
        IPetFinderClient petFinderClient,
        HttpContext httpContext
    )
    {
        ErrorOr<PagedList<PetDto>> petsResult = await petFinderClient.GetPetsAsync(petsParams);
        if (petsResult.IsError)
            return GetProblemHttpResult(petsResult.FirstError);

        List<PetDto> petDtoList = petsResult.Value.Items;
        PaginationMetaData paginationMetaData = petsResult.Value.Pagination;
        httpContext.Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetaData));

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
    [ProducesResponseType<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest,
        MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    [Tags("pets")]
    public static async Task<Results<Ok<PetDto>, ProblemHttpResult>> GetSinglePetAsync(
        [FromRoute] int id,
        IPetFinderClient petFinderClient
    )
    {
        ErrorOr<PetDto> petResult = await petFinderClient.GetSinglePetAsync(id);
        return petResult.IsError
            ? GetProblemHttpResult(petResult.FirstError)
            : TypedResults.Ok(petResult.Value);
    }
}