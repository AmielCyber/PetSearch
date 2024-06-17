using ErrorOr;
using Microsoft.Extensions.Options;
using PetSearch.API.Exceptions;
using PetSearch.API.Models;
using PetSearch.API.Models.MapBoxResponse;
using PetSearch.API.Configurations;

namespace PetSearch.API.Clients;

/// <summary>
/// MapBoxClient implementation to handle requests from the MapBox API.
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
    public MapBoxClient(HttpClient httpClient, IOptions<MapBoxConfiguration> options)
    {
        _httpClient = httpClient;
        MapBoxConfiguration keys = options.Value;
        Dictionary<string, string> queryParameters = keys.QueryParameters;

        _queryFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
    }

    /// <summary>
    /// Requests MapBox API to retrieve a zipcode's location information such as the city,state, and country.
    /// </summary>
    /// <param name="zipcode">The zipcode to get its location information.</param>
    /// <returns>Location object if request is successful or an Error type if response was unsuccessful.</returns>
    /// <exception cref="ForbiddenAccessException">If we are denied from using MapBox.</exception>
    public async Task<ErrorOr<LocationDto>> GetLocationFromZipCode(string zipcode)
    {
        // TODO: Avoid ReadAsString for every request.
        string queryString = await _queryFormUrlEncoded.ReadAsStringAsync();
        using HttpResponseMessage response = await _httpClient.GetAsync($"{zipcode}.json?{queryString}");

        return await ExtractLocationDto(response);
    }

    /// <summary>
    /// Requests MapBox API to retrieve the coordinate's location information such as the city,state, and country.
    /// Used for the browser geolocation API when it prompts the user to share their location.
    /// </summary>
    /// <param name="longitude">Longitude double value.</param>
    /// <param name="latitude">Latitude double value.</param>
    /// <returns>Location object if request is successful or an Error type if response was unsuccessful.</returns>
    /// <exception cref="ForbiddenAccessException">If we are denied from using MapBox.</exception>
    public async Task<ErrorOr<LocationDto>> GetLocationFromCoordinates(double longitude, double latitude)
    {
        // TODO: Avoid ReadAsString for every request.
        string queryString = await _queryFormUrlEncoded.ReadAsStringAsync();
        using HttpResponseMessage response = await _httpClient
            .GetAsync($"{longitude},{latitude}.json?{queryString}");

        return await ExtractLocationDto(response);
    }

    /// <summary>
    /// Extract the Location information needed from the MapBox API response.
    /// </summary>
    /// <param name="response">The MapBox response</param>
    /// <returns>Location object if successful or an Error type if response was not successful.</returns>
    /// <exception cref="ForbiddenAccessException">If we are denied from using MapBox.</exception>
    private static async Task<ErrorOr<LocationDto>> ExtractLocationDto(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            return GetLocationError((int)response.StatusCode);

        MapBoxResponse? mapBoxResponse = await response.Content.ReadFromJsonAsync<MapBoxResponse>();

        if (mapBoxResponse is null)
            return GetLocationError(500);

        MapBoxFeatures? features = mapBoxResponse.FeaturesList.FirstOrDefault();
        if (features is null)
            return GetLocationError(404);

        return new LocationDto(features.Zipcode, features.LocationName);
    }

    /// <summary>
    /// Gets a custom ErrorOr Error while requesting an endpoint from the MapBox API.
    /// </summary>
    /// <param name="statusCode">The request status code that we got from an error.</param>
    /// <returns>ErrorOr Error type.</returns>
    /// <exception cref="ForbiddenAccessException">If we are denied from using MapBox then we should log it with our exception middleware.</exception>
    private static Error GetLocationError(int statusCode)
    {
        if (statusCode == 403)
        {
            // Throw exception since its unexpected and something we have to handle on our end, since our
            // client app handles auto refresh token.
            throw new ForbiddenAccessException("Forbidden response generated from MapBox API");
        }

        return statusCode switch
        {
            400 => Errors.Errors.Location.BadRequest,
            401 => Errors.Errors.Token.Unauthorized,
            404 => Errors.Errors.Location.NotFound,
            _ => Errors.Errors.Location.ServerError,
        };
    }
}