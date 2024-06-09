using Microsoft.Extensions.Caching.Memory;
using PetSearch.Data.Entities;

namespace PetSearch.Data.Services;

/// <summary>
/// Cache token service to avoid maxing our daily token requests.
/// </summary>
public class CachedTokenService: ITokenService
{
    private const string TokenCacheKey = "token";
    private readonly TokenService _tokenService;
    private readonly IMemoryCache _memoryCache;

    public CachedTokenService(TokenService tokenService, IMemoryCache memoryCache)
    {
        _tokenService = tokenService;
        _memoryCache = memoryCache;
    }

    /// <summary>
    /// Gets token from either cache or token service.
    /// </summary>
    /// <returns></returns>
    public async Task<Token> GetToken()
    {
        Token? token = _memoryCache.Get<Token>(TokenCacheKey);
        if (token is not null) return token;

        token = await _tokenService.GetToken();
        
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(token.ExpiresIn);
        _memoryCache.Set(TokenCacheKey, token, cacheEntryOptions);

        return token;
    }
}