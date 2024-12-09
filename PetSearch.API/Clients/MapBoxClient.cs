using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using PetSearch.API.Exceptions;
using PetSearch.API.Models;
using PetSearch.API.Models.MapBoxResponse;
using PetSearch.API.Configurations;
using PetSearch.API.Problems;

namespace PetSearch.API.Clients;

/// <summary>
/// MapBoxClient implementation to handle requests from the MapBox API.
/// </summary>
public class MapBoxClient : IMapBoxClient
{
    private readonly HttpClient _httpClient;
    private readonly string _optionsQuery;
    private readonly IExpectedProblems _expectedProblems;

    /// <summary>
    /// Set up dependency injection.
    /// </summary>
    /// <param name="httpClient">Have access to the global HttpClient object to make external API requests.</param>
    /// <param name="options">The configuration options for using the MapBox API.</param>
    /// <param name="expectedProblems">MapBox expected problems to return if response fails.</param>
    public MapBoxClient(HttpClient httpClient, IOptions<MapBoxConfiguration> options,
        [FromKeyedServices("mapBoxProblems")] IExpectedProblems expectedProblems)
    {
        _httpClient = httpClient;
        _expectedProblems = expectedProblems;

        MapBoxConfiguration keys = options.Value;
        _optionsQuery = keys.OptionsQuery;
    }

    /// <summary>
    /// Requests MapBox API to retrieve a zipcode's location information such as the city,state, and country.
    /// </summary>
    /// <param name="zipcode">The zipcode to get its location information.</param>
    /// <returns>Location object if request is successful or an Error type if response was unsuccessful.</returns>
    /// <exception cref="ForbiddenAccessException">If we are denied from using the MapBox API.</exception>
    public async Task<Results<Ok<LocationDto>, ProblemHttpResult>> GetLocationFromZipCode(string zipcode)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync($"{zipcode}.json{_optionsQuery}");
        return await GetLocationResponse(response);
    }

    /// <summary>
    /// Requests MapBox API to retrieve the coordinate's location information such as the city,state, and country.
    /// Used for the browser geolocation API when it prompts the user to share their location.
    /// </summary>
    /// <param name="longitude">Longitude double value.</param>
    /// <param name="latitude">Latitude double value.</param>
    /// <returns>Location object if request is successful or an Error type if response was unsuccessful.</returns>
    /// <exception cref="ForbiddenAccessException">If we are denied from using the MapBox API.</exception>
    public async Task<Results<Ok<LocationDto>, ProblemHttpResult>> GetLocationFromCoordinates(double longitude,
        double latitude)
    {
        using HttpResponseMessage response = await _httpClient
            .GetAsync($"{longitude},{latitude}.json{_optionsQuery}");

        return await GetLocationResponse(response);
    }

    /// <summary>
    /// Extract the Location information needed from the MapBox API response.
    /// </summary>
    /// <param name="response">The MapBox response</param>
    /// <returns>Location object if successful or an Error type if response was not successful.</returns>
    /// <exception cref="ForbiddenAccessException">If we are denied from using MapBox.</exception>
    private async Task<Results<Ok<LocationDto>, ProblemHttpResult>> GetLocationResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            return ClientProblems.GetProblemHttpResult(response.StatusCode, _expectedProblems);

        MapBoxResponse? mapBoxResponse = await response.Content.ReadFromJsonAsync<MapBoxResponse>();
        if (mapBoxResponse is null)
            return ClientProblems.GetProblemHttpResult(HttpStatusCode.InternalServerError, _expectedProblems);

        MapBoxFeatures? features = mapBoxResponse.FeaturesList.FirstOrDefault();
        if (features is null)
            return ClientProblems.GetProblemHttpResult(HttpStatusCode.NotFound, _expectedProblems);

        return TypedResults.Ok(new LocationDto(features.Zipcode, features.LocationName));
    }
}