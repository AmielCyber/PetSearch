using PetSearchAPI.Common.Exceptions;
using PetSearchAPI.Models.Token;

namespace PetSearchAPI.Clients;

public class TokenClient: ITokenClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _client;

    public TokenClient(IConfiguration configuration, HttpClient client)
    {
        _configuration = configuration;
        _client = client;
    }
    
    /// <summary>
    /// Gets token for the client application in order to call our endpoints.
    /// </summary>
    /// <returns>Token response body</returns>
    /// <exception cref="TokenFetchException">Throws if we failed to obtain a token from PetFinder API</exception>
    public async Task<TokenResponseDto> GetToken()
    {
        var requestBody = new TokenRequestBody
        {
            ClientId = _configuration["PetFinder:ClientId"],
            ClientSecret = _configuration["PetFinder:ClientSecret"]
        };

        using HttpResponseMessage response = await _client.PostAsJsonAsync("",requestBody);

        if (!response.IsSuccessStatusCode)
        {
            // Throw the exception since that is something that happened in our end or the api (500).
            throw new TokenFetchException();
        }

        TokenResponseDto petFinderToken = await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        return petFinderToken;
    }
}