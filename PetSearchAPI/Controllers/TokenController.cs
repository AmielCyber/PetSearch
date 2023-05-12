using Microsoft.AspNetCore.Mvc;
using PetSearchAPI.Clients;
using PetSearchAPI.Models.Token;

namespace PetSearchAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TokenController : ControllerBase
{
    private readonly IPetFinderClient _petFinderClient;

    public TokenController(IPetFinderClient petFinderClient)
    {
        _petFinderClient = petFinderClient;
    }
    
    [HttpGet]
    public async Task<ActionResult<TokenResponseDto>> GetToken()
    {
        return await _petFinderClient.GetToken();
    }
}