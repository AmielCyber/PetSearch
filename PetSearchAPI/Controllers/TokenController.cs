using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using PetSearchAPI.Clients;
using PetSearchAPI.Common.Errors;
using PetSearchAPI.Models.Token;
namespace PetSearchAPI.Controllers;

/// <summary>
/// Token Client to fetch a token for our client app.
/// </summary>
[Produces("application/json")]
public class TokenController : ApiController
{
    private readonly IPetFinderClient _petFinderClient;

    public TokenController(IPetFinderClient petFinderClient)
    {
        _petFinderClient = petFinderClient;
    }
    
    /// <summary>
    /// Get: /api/token
    /// Gets a token for our client app.
    /// </summary>
    /// <returns>A Token Response DTO</returns>
    [HttpGet]
    public async Task<ActionResult<TokenResponseDto>> GetToken()
    {
        TokenResponseDto tokenResponseDto = await _petFinderClient.GetToken();
        return tokenResponseDto;
    }
}