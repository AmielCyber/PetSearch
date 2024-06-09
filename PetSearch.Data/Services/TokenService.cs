using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetSearch.Data.Common.Exceptions;
using PetSearch.Data.Entities;
using PetSearch.Data.Models;
using PetSearch.Data.StronglyTypedConfigurations;

namespace PetSearch.Data.Services;

/// <summary>
/// TokenService implementation for storing and retrieving token from our DB.
/// </summary>
public class TokenService : ITokenService
{
    private const int TokenId = 1;
    private readonly PetSearchContext _context;
    private readonly HttpClient _client;
    private readonly TokenRequestBody _requestBody;

    public TokenService(PetSearchContext context, HttpClient client, IOptions<PetFinderConfiguration> options)
    {
        _context = context;
        _client = client;
        PetFinderConfiguration keys = options.Value;
        string? clientId = keys.ClientId;
        string? clientSecret = keys.ClientSecret;

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            throw new TokenFetchException("User secrets not set up!");
        }

        _requestBody = new TokenRequestBody(clientId, clientSecret);
    }

    /// <summary>
    /// Gets token from our DB if there is one, else it will create a new token from the PetFinder API.
    /// </summary>
    /// <returns>Token Object</returns>
    public async Task<Token> GetToken()
    {
        Token token = await _context.Tokens.FirstOrDefaultAsync(t => t.Id == TokenId) ?? await CreateToken();

        if (TokenIsExpired(token))
        {
            await UpdateToken(token);
        }

        return token;
    }

    /// <summary>
    /// Creates new token and gets access token from the PetFinder's API.
    /// </summary>
    /// <returns>Token Object</returns>
    /// <exception cref="TokenUpdateException"></exception>
    private async Task<Token> CreateToken()
    {
        DateTime expiresIn = DateTime.Now.AddMinutes(55);
        TokenResponseDto petFinderToken = await GetNewTokenFromPetFinder();

        Token token = new Token
        {
            Id = TokenId,
            AccessToken = petFinderToken.AccessToken,
            ExpiresIn = expiresIn
        };

        return await StoreAndRetrieveTokenInDataBase(token);
    }


    private bool TokenIsExpired(Token token)
    {
        return DateTime.Now >= token.ExpiresIn;
    }

    private async Task UpdateToken(Token token)
    {
        DateTime expiresIn = DateTime.Now.AddMinutes(55);
        TokenResponseDto petFinderToken = await GetNewTokenFromPetFinder();


        token.AccessToken = petFinderToken.AccessToken;
        token.ExpiresIn = expiresIn;
        _context.Tokens.Update(token);

        var saved = await _context.SaveChangesAsync() > 0;

        if (!saved)
        {
            throw new TokenUpdateException("Failed to save changes.");
        }
    }

    private async Task<TokenResponseDto> GetNewTokenFromPetFinder()
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
    
    private async Task<Token> StoreAndRetrieveTokenInDataBase(Token token)
    {
        var result = await _context.Tokens.AddAsync(token);
        var saved = await _context.SaveChangesAsync() > 0;
        if (!saved)
        {
            throw new TokenUpdateException("Failed to save changes.");
        }
        return result.Entity;
    }
}