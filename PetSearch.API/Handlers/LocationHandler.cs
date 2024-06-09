using System.ComponentModel.DataAnnotations;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using PetSearch.API.Clients;
using PetSearch.API.Models.MapBoxResponse;

namespace PetSearch.API.Handlers;

/// <summary>
/// Handler function for endpoint: "/api/location"
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
    /// <param name="mapBoxClient">Dependency Injection using IMapBoxClient</param>
    /// <returns>Location object containing the zipcode and the location information.</returns>
    /// <response code="200">Returns the location object.</response>
    /// <response code="400">If the zipcode is invalid.</response>
    /// <response code="404">No location found with the given zipcode.</response>
    [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status400BadRequest,
        "application/problem+json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, "application/problem+json")]
    [Tags("location")]
    public static async Task<IResult> GetLocationFromZipCode(
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be 5 digits.")] [FromRoute]
        string zipcode,
        [FromServices] IMapBoxClient mapBoxClient
    )
    {
        ErrorOr<LocationDto> locationResult = await mapBoxClient.GetLocationFromZipCode(zipcode);
        return locationResult.Match(TypedResults.Ok, GetProblems);
    }

    /// <summary>
    /// Gets the location information from coordinates.
    /// </summary>
    /// <remarks>
    /// Gets the location information from coordinates, such as the city, state, zipcode, and its country.
    /// </remarks>
    /// <param name="longitude">Longitude coordinate value.</param>
    /// <param name="latitude">Latitude coordinate value.</param>
    /// <param name="mapBoxClient">Dependency Injection using IMapBoxClient</param>
    /// <returns>Location object containing the zipcode and the location information.</returns>
    /// <response code="200">Returns the location object.</response>
    /// <response code="400">If the coordinates are invalid.</response>
    /// <response code="404">No location found with the given coordinates.</response>
    [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), StatusCodes.Status400BadRequest,
        "application/problem+json")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, "application/problem+json")]
    [Tags("location")]
    public static async Task<IResult> GetLocationFromCoordinates([FromQuery] double longitude,
        [FromQuery] double latitude, [FromServices] IMapBoxClient mapBoxClient)
    {
        ErrorOr<LocationDto> locationResult = await mapBoxClient.GetLocationFromCoordinates(longitude, latitude);
        return locationResult.Match(TypedResults.Ok, GetProblems);
    }
}