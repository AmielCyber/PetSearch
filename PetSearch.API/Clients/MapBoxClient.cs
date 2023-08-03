using ErrorOr;
using Microsoft.Extensions.Options;
using PetSearch.API.Common.Errors;
using PetSearch.API.Common.Exceptions;
using PetSearch.API.Models.MabBoxResponse;
using PetSearch.API.StronglyTypedConfigurations;

namespace PetSearch.API.Clients;

/// <summary>
/// MapBoxClient implementation to handle requests from the MapBox API.
/// Returns Location object to our client app.
/// </summary>
public class MapBoxClient : IMapBoxClient
{
    private readonly HttpClient _httpClient;
    private readonly FormUrlEncodedContent _queryFormUrlEncoded;

    /// <summary>
    /// Set up dependency injection.
    /// </summary>
    /// <param name="httpClient">Have access to the global HttpClient object to make external API requests.</param>
    /// <param name="options">The configuration options for using the MapBox API.</param>
    /// <exception cref="MissingMapBoxToken">If configuration options was not set up.</exception>
    public MapBoxClient(HttpClient httpClient, IOptions<MapBoxConfiguration> options)
    {
        _httpClient = httpClient;
        MapBoxConfiguration keys = options.Value;
        string? accessToken = keys.AccessToken;

        if (string.IsNullOrEmpty(accessToken))
        {
            throw new MissingMapBoxToken();
        }

        var queryParameters = new Dictionary<string, string>
        {
            { "country", "us" },
            { "limit", "1" },
            { "types", "postcode" },
            { "language", "en" },
            { "access_token", accessToken }
        };
        _queryFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
    }

    /// <summary>
    /// Calls the MapBox API to retrieve the zipcode's location information such as the city,state, and country.
    /// </summary>
    /// <param name="zipcode">Five digit zipcode to get location information.</param>
    /// <returns>Location object if successful or an Error type if response was not successful.</returns>
    /// <exception cref="MapBoxForbidden">If we are denied from using MapBox then we should log it with our exception middleware.</exception>
    public async Task<ErrorOr<LocationDto>> GetLocationFromZipCode(string zipcode)
    {
        string queryString = await _queryFormUrlEncoded.ReadAsStringAsync();
        using HttpResponseMessage response = await _httpClient.GetAsync($"{zipcode}.json?{queryString}");

        if (!response.IsSuccessStatusCode)
        {
            return GetLocationError((int)response.StatusCode);
        }

        return await ExtractLocationDto(response);
    }

    /// <summary>
    /// Calls the MapBox API to retrieve the coordinate's location information such as the city,state, and country.
    /// Used for the browser geolocation API when it prompts the user to share their location.
    /// </summary>
    /// <param name="longitude">Longitude double value.</param>
    /// <param name="latitude">Latitude double value.</param>
    /// <returns>Location object if successful or an Error type if response was not successful.</returns>
    /// <exception cref="MapBoxForbidden">If we are denied from using MapBox then we should log it with our exception middleware.</exception>
    public async Task<ErrorOr<LocationDto>> GetLocationFromCoordinates(double longitude, double latitude)
    {
        string queryString = await _queryFormUrlEncoded.ReadAsStringAsync();
        using HttpResponseMessage response = await _httpClient.GetAsync($"{longitude},{latitude}.json?{queryString}");

        if (!response.IsSuccessStatusCode)
        {
            return GetLocationError((int)response.StatusCode);
        }

        return await ExtractLocationDto(response);
    }

    /// <summary>
    /// Extract the Location information needed from the MapBox API response.
    /// </summary>
    /// <param name="response">The MapBox response</param>
    /// <returns>Location object if successful or an Error type if response was not successful.</returns>
    /// <exception cref="MapBoxForbidden">If we are denied from using MapBox then we should log it with our exception middleware.</exception>
    private static async Task<ErrorOr<LocationDto>> ExtractLocationDto(HttpResponseMessage response)
    {
        MapBoxResponse? mapBoxResponse = await response.Content.ReadFromJsonAsync<MapBoxResponse>();

        if (mapBoxResponse is null)
        {
            return GetLocationError(500);
        }

        MapBoxFeatures? features = mapBoxResponse.FeaturesList.FirstOrDefault();
        if (features is null)
        {
            return GetLocationError(404);
        }

        return new LocationDto(features.Zipcode, features.LocationName);
    }

    /// <summary>
    /// Gets a custom ErrorOr Error while requesting an endpoint from the MapBox API.
    /// </summary>
    /// <param name="statusCode">The request status code that we got from an error.</param>
    /// <returns>ErrorOr Error type.</returns>
    /// <exception cref="MapBoxForbidden">If we are denied from using MapBox then we should log it with our exception middleware.</exception>
    private static Error GetLocationError(int statusCode)
    {
        if (statusCode == 403)
        {
            // Throw exception since its unexpected and something we have to handle on our end, since our
            // client app handles auto refresh token.
            // Is catch by our global error handler and will log exception.
            throw new MapBoxForbidden();
        }

        return statusCode switch
        {
            400 => Errors.Location.BadRequest,
            401 => Errors.Token.NotAuthorized,
            404 => Errors.Location.NotFound,
            _ => Errors.Location.ServerError,
        };
    }
}