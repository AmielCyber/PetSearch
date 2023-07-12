using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using PetSearch.API.Clients;
using PetSearch.API.Models.PetFinderResponse;
using PetSearch.API.RequestHelpers;

namespace PetSearch.API.Controllers;

/// <summary>
/// Pets controller endpoint that will fetch pet data and send it to the client.
/// </summary>
[Produces(MediaTypeNames.Application.Json, "application/json+problem")]
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
    /// <returns>Returns a list available pets and the pagination object if successful, else returns a
    /// problem detail.</returns>
    /// <response code="200">Returns the pet list and pagination metadata.</response>
    /// <response code="400">If search params has invalid values.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PetsResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetPets([FromQuery] PetsParams petsParams)
    {
        var petsResult = await _petFinderClient.GetPets(petsParams);
        return petsResult.Match(Ok, GetProblems);
    }

    // GET: /api/pets/{id}
    /// <summary>
    /// Gets a single pet object containing the pet's description.
    /// </summary>
    /// <param name="id">The id of the pet object to fetch.</param>
    /// <returns>A pet object if request was successful, else a problem detail.</returns>
    /// <response code="200">Returns the pet object.</response>
    /// <response code="400">If id is not an integer.</response>
    /// <response code="404">If pet with passed id is not found or has been adopted.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PetDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPets(int id)
    {
        var petResult = await _petFinderClient.GetSinglePet(id);
        return petResult.Match(Ok, GetProblems);
    }
}