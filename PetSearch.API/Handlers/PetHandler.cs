using Microsoft.AspNetCore.Mvc;
using PetSearch.API.Clients;
using PetSearch.API.Models.PetFinderResponse;
using PetSearch.API.RequestHelpers;

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
    /// <returns>
    /// Returns a list available pets and the pagination object if successful, else returns a
    /// problem detail.
    /// </returns>
    /// <response code="200">Returns the pet list from the search query and its pagination metadata.</response>
    /// <response code="400">If search parameters contains invalid values.</response>
    [ProducesResponseType(typeof(PetsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status400BadRequest,
        "application/problem+json")]
    [Tags("pets")]
    public static async Task<IResult> GetPets([AsParameters] PetsParams petsParams,
        [FromServices] IPetFinderClient petFinderClient)
    {
        var petsResult = await petFinderClient.GetPets(petsParams);
        return petsResult.Match(TypedResults.Ok, GetProblems);
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
    [ProducesResponseType(typeof(PetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status400BadRequest,
        "application/problem+json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, "application/problem+json")]
    [Tags("pets")]
    public static async Task<IResult> GetSinglePet([FromRoute] int id, [FromServices] IPetFinderClient petFinderClient)
    {
        var petResult = await petFinderClient.GetSinglePet(id);
        return petResult.Match(TypedResults.Ok, GetProblems);
    }
}