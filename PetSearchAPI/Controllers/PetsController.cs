using Microsoft.AspNetCore.Mvc;
using PetSearchAPI.Clients;
using PetSearchAPI.Models.PetFinderResponse;
using PetSearchAPI.RequestHelpers;

namespace PetSearchAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PetsController: ControllerBase
{
    private readonly IPetFinderClient _petFinderClient;

    public PetsController(IPetFinderClient petFinderClient)
    {
        _petFinderClient = petFinderClient;
    }
    
    // GET: /api/pets
    [HttpGet]
    public async Task<ActionResult<PetsResponseDto>> GetPets([FromQuery]PetsParams petsParams, [FromHeader]string authorization)
    {
        if (string.IsNullOrEmpty(authorization))
        {
            return Unauthorized("Bearer token needed to access this endpoint.");
        }
        var pets = await _petFinderClient.GetPets(petsParams, authorization);
        return Ok(pets);
    }
    
    // GET: /api/pets/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<PetDto>> GetPets(int id, [FromHeader]string authorization)
    {
        if (string.IsNullOrEmpty(authorization))
        {
            return Unauthorized("Bearer token needed to access this endpoint.");
        }
        var pet = await _petFinderClient.GetSinglePet(id, authorization);
        return Ok(pet);
    }
}