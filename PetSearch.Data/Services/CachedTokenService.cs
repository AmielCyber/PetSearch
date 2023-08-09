using Microsoft.Extensions.Caching.Memory;
using PetSearch.Data.Entity;

namespace PetSearch.Data.Services;

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