using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetSearch.API.Clients;
using PetSearch.API.Models;

namespace PetSearch.API.Handlers;

/// <summary>
/// Handler function for the endpoint: "/api/location"
/// </summary>
public class LocationHandler 
{
    /// <summary>
    /// Gets the location information from a zipcode.
    /// </summary>
    /// <remarks>
    /// Gets the location information from a zipcode, such as the city, state, zipcode, and its country.
    /// </remarks>
    /// <param name="zipcode">A five digit zipcode.</param>
    /// <param name="mapBoxClient"> IMapBoxClient that will handle the api call to MapBox API and retrieves the result
    /// </param>
    /// <returns>Location object containing the zipcode and the location information.</returns>
    /// <response code="200">Returns the location object.</response>
    /// <response code="400">If the given zipcode is invalid.</response>
    /// <response code="404">No location found with the given zipcode.</response>
    public static async Task<Results<Ok<LocationDto>, ProblemHttpResult>> GetLocationFromZipCodeAsync(
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be 5 digits.")] [FromRoute]
        string zipcode,
        IMapBoxClient mapBoxClient
    )
    {
        return await mapBoxClient.GetLocationFromZipCode(zipcode);
    }

    /// <summary>
    /// Gets the location information from the longitude and latitude coordinates.
    /// </summary>
    /// <remarks>
    /// Gets the location information from coordinates, such as the city, state, zipcode, and its country.
    /// </remarks>
    /// <param name="longitude">Longitude coordinate value.</param>
    /// <param name="latitude">Latitude coordinate value.</param>
    /// <param name="mapBoxClient"> IMapBoxClient that will handle the api call to MapBox API and retrieves the result
    /// </param>
    /// <returns>Location object containing the zipcode and the location information.</returns>
    /// <response code="200">Returns the location object.</response>
    /// <response code="400">If the given zipcode is invalid.</response>
    /// <response code="404">No location found with the given zipcode.</response>
    public static async Task<Results<Ok<LocationDto>, ProblemHttpResult>> GetLocationFromCoordinatesAsync(
        [FromQuery] double longitude,
        [FromQuery] double latitude,
        IMapBoxClient mapBoxClient
    )
    {
        return await mapBoxClient.GetLocationFromCoordinates(longitude, latitude);

    }
}