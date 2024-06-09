namespace PetSearch.API.StronglyTypedConfigurations;

/// <summary>
/// MapBoxConfiguration for storing access token in a Singleton.
/// </summary>
public class MapBoxConfiguration
{
    private const string AccessTokenKey = "access_token";

    private readonly Dictionary<string, string> _queryParameters = new()
    {
        { "country", "us" },
        { "limit", "1" },
        { "types", "postcode" },
        { "language", "en" },
    };

    public required string AccessToken { init; get; }

    public Dictionary<string, string> QueryParameters
    {
        get
        {
            _queryParameters.TryAdd(AccessTokenKey, AccessToken);
            return _queryParameters;
        }
    }
}