using System.Collections.Immutable;
using Microsoft.AspNetCore.WebUtilities;

namespace PetSearch.API.Configurations;

/// <summary>
/// MapBoxConfiguration for obtaining MapBox API options such as access token.
/// </summary>
public class MapBoxConfiguration
{
    /// <summary>
    /// The endpoint to call the MapBoxApi
    /// </summary>
    public const string Url = "https://api.mapbox.com/geocoding/v5/mapbox.places/";

    /// <summary>
    /// Options query to get the desired result along with our access token.
    /// </summary>
    public readonly string OptionsQuery = "";

    /// <summary>
    /// Access token to have asp net configuration initialize.
    /// </summary>
    public required string AccessToken
    {
        init
        {
            IDictionary<string, string?> options = new Dictionary<string, string?>()
            {
                { "country", "us" },
                { "limit", "1" },
                { "types", "postcode" },
                { "language", "en" },
                { "access_token", value }
            }.ToImmutableDictionary();
            _accessToken = value;
            OptionsQuery = QueryHelpers.AddQueryString(string.Empty, options);
        }
        get => _accessToken;
    }

    private readonly string _accessToken = String.Empty;
}