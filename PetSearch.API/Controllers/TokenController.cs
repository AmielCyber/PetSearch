using Microsoft.AspNetCore.Mvc;
using PetSearchAPI.Clients;
using PetSearchAPI.Models.Token;

namespace PetSearchAPI.Controllers;

/// <summary>
/// Token Client to fetch a token for our client app.
/// </summary>
[Produces("application/json", "application/json+problem")]
public class TokenController : ApiController
{
    private readonly ITokenClient _tokenClient;

    public TokenController(ITokenClient tokenClient)
    {
        _tokenClient = tokenClient;
    }

    /// <summary>
    /// Get: /api/token
    /// Gets a token for our client app.
    /// </summary>
    /// <returns>A Token Response DTO</returns>
    /// <response code="200">Returns the token object.</response>
    [HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<TokenResponseDto>> GetToken()
    {
        TokenResponseDto tokenResponseDto = await _tokenClient.GetToken();
        return Ok(tokenResponseDto);
    }
}