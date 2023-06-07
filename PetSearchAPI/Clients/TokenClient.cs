using PetSearchAPI.Common.Exceptions;
using PetSearchAPI.Models.Token;

namespace PetSearchAPI.Clients;

/// <summary>
/// TokenClient implementation to request tokens from the PetFinderApi.
/// </summary>
public class TokenClient : ITokenClient
{
    private readonly HttpClient _client;
    private readonly TokenRequestBody _requestBody;

    /// <summary>
    /// Set up dependency injection.
    /// </summary>
    /// <param name="configuration">Configuration client that holds our keys.</param>
    /// <param name="client">Global HttpClient we use to use external APIs</param>
    public TokenClient(IConfiguration configuration, HttpClient client)
    {
        _client = client;
        string? clientId = configuration["PetFinder:ClientId"];
        string? clientSecret = configuration["PetFinder:ClientSecret"];
        
        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            throw new TokenFetchException("User secrets not set up!");
        }

        _requestBody = new TokenRequestBody
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };
    }

    /// <summary>
    /// Gets token for the client application in order to call our endpoints.
    /// </summary>
    /// <returns>Token response body</returns>
    /// <exception cref="TokenFetchException">Throws if we failed to obtain a token from PetFinder API</exception>
    public async Task<TokenResponseDto> GetToken()
    {
        using HttpResponseMessage response = await _client.PostAsJsonAsync("", _requestBody);

        if (!response.IsSuccessStatusCode)
        {
            // Throw the exception since that is something that happened in our end or the api (500).
            // Is catch by our global error handler and will log exception.
            throw new TokenFetchException("Failed to get token from PetFinder API.");
        }

        TokenResponseDto? petFinderToken = await response.Content.ReadFromJsonAsync<TokenResponseDto>();

        if (petFinderToken is null)
        {
            throw new TokenFetchException("Failed to return a TokenResponseDto object.");
        }
        
        return petFinderToken;
    }
}