using Microsoft.AspNetCore.Mvc;
using PetSearchAPI.Clients;
using PetSearchAPI.Models.PetFinderResponse;
using PetSearchAPI.RequestHelpers;

namespace PetSearchAPI.Controllers;

/// <summary>
/// Pets controller endpoint that will fetch pet data and send it to the client.
/// </summary>
[Produces("application/json")]
public class PetsController: ApiController
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
    
    /// <summary>
    /// GET: /api/pets.
    /// Gets a list of available pets based on the search query.
    /// </summary>
    /// <param name="petsParams">Pets parameters for a search query.</param>
    /// <param name="authorization">Header access token to access endpoint.</param>
    /// <returns>Returns a list available pets and the pagination object if successful, else returns a
    /// problem detail.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(PetsResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPets([FromQuery]PetsParams petsParams, [FromHeader]string authorization)
    {
        var petsResult = await _petFinderClient.GetPets(petsParams, authorization);
        return petsResult.Match(Ok, GetProblems);
    }
    
    /// <summary>
    /// GET: /api/pets/{id}
    /// Gets a single pet object.
    /// </summary>
    /// <param name="id">The id of the pet object we wish to fetch.</param>
    /// <param name="authorization">Header access token to access endpoint.</param>
    /// <returns>A pet object if request was successful, else a problem detail.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PetDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPets(int id, [FromHeader]string authorization)
    {
        var petResult = await _petFinderClient.GetSinglePet(id, authorization);
        return petResult.Match(Ok, GetProblems);
    }
}