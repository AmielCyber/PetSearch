using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PetSearch.API.Clients;
using PetSearch.API.Models;

namespace PetSearch.API.Handlers;

/// <summary>
/// Handler function for the endpoint: "/api/location"
/// </summary>
public class LocationHandler : BaseHandler
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
    [ProducesResponseType<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest,
        MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    [Tags("location")]
    public static async Task<Results<Ok<LocationDto>, ProblemHttpResult>> GetLocationFromZipCode(
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be 5 digits.")] [FromRoute]
        string zipcode,
        [FromServices] IMapBoxClient mapBoxClient
    )
    {
        ErrorOr<LocationDto> locationResult = await mapBoxClient.GetLocationFromZipCode(zipcode);

        return locationResult.IsError
            ? GetProblemHttpResult(locationResult.FirstError)
            : TypedResults.Ok(locationResult.Value);
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
    [ProducesResponseType<HttpValidationProblemDetails>(StatusCodes.Status400BadRequest,
        MediaTypeNames.Application.ProblemJson)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound, MediaTypeNames.Application.ProblemJson)]
    [Tags("location")]
    public static async Task<Results<Ok<LocationDto>, ProblemHttpResult>> GetLocationFromCoordinates(
        [FromQuery] double longitude,
        [FromQuery] double latitude,
        [FromServices] IMapBoxClient mapBoxClient
    )
    {
        ErrorOr<LocationDto> locationResult = await mapBoxClient.GetLocationFromCoordinates(longitude, latitude);

        return locationResult.IsError
            ? GetProblemHttpResult(locationResult.FirstError)
            : TypedResults.Ok(locationResult.Value);
    }
}