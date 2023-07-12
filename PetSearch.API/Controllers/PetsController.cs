using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using PetSearchAPI.Clients;
using PetSearchAPI.Filters;
using PetSearchAPI.Models.PetFinderResponse;
using PetSearchAPI.RequestHelpers;

namespace PetSearchAPI.Controllers;

/// <summary>
/// Pets controller endpoint that will fetch pet data and send it to the client.
/// </summary>
/// <response code="401">If token is missing, expired, or invalid.</response>
[Produces(MediaTypeNames.Application.Json, "application/json+problem")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[PetAuthorizationFilter]
public class PetsController : ApiController
{
    private readonly IPetFinderClient _petFinderClient;

    /// <summary>
    /// Inject PetFinder dependency that will handle all HTTP requests to PetFinderApi.
    /// </summary>
    /// <param name="petFinderClient">Handles all HTTP requests.</param>
    public PetsController(IPetFinderClient petFinderClient)
    {
        _petFinderClient = petFinderClient;
    }

    // GET: /api/pets.
    /// <summary>
    /// Get a list of available pets based on the search query.
    /// </summary>
    /// <param name="petsParams">Pets parameters for the search query.</param>
    /// <param name="authorization">Header bearer access token to access endpoint.</param>
    /// <returns>Returns a list available pets and the pagination object if successful, else returns a
    /// problem detail.</returns>
    /// <response code="200">Returns the pet list and pagination metadata.</response>
    /// <response code="400">If search params has invalid values.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PetsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPets([FromQuery] PetsParams petsParams, [FromHeader] string authorization)
    {
        var petsResult = await _petFinderClient.GetPets(petsParams, authorization);
        return petsResult.Match(Ok, GetProblems);
    }

    // GET: /api/pets/{id}
    /// <summary>
    /// Gets a single pet object containing the pet's description.
    /// </summary>
    /// <param name="id">The id of the pet object to fetch.</param>
    /// <param name="authorization">Header access bearer token to access endpoint.</param>
    /// <returns>A pet object if request was successful, else a problem detail.</returns>
    /// <response code="200">Returns the pet object.</response>
    /// <response code="400">If id is not an integer.</response>
    /// <response code="404">If pet with passed id is not found or has been adopted.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPets(int id, [FromHeader] string authorization)
    {
        var petResult = await _petFinderClient.GetSinglePet(id, authorization);
        return petResult.Match(Ok, GetProblems);
    }
}