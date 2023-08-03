using System.ComponentModel.DataAnnotations;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using PetSearch.API.Clients;
using PetSearch.API.Models.MabBoxResponse;

namespace PetSearch.API.Controllers;

/// <summary>
/// Location controller endpoint that will fetch the location information depending if the user
/// passed a zipcode or coordinates.
/// </summary>
public class LocationController : ApiController
{
    private readonly IMapBoxClient _mapBoxClient;

    /// <summary>
    /// Inject MapBox dependency that will handle all HTTP requests to MapBoxAPI.
    /// </summary>
    /// <param name="mapBoxClient">Handles all HTTP requests.</param>
    public LocationController(IMapBoxClient mapBoxClient)
    {
        _mapBoxClient = mapBoxClient;
    }

    // GET: /api/Location/Zipcode/
    /// <summary>
    /// Gets the location information from a zipcode, such as the city, state, zipcode, and its country.
    /// </summary>
    /// <param name="zipcode">A five digit zipcode.</param>
    /// <returns>Location object containing the zipcode and the location information.</returns>
    /// <response code="200">Returns the location object.</response>
    /// <response code="400">If the zipcode is invalid.</response>
    /// <response code="404">No location found with the given zipcode.</response>
    [HttpGet("Zipcode/{zipcode}")]
    [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocation(
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be 5 digits.")]
        string zipcode)
    {
        ErrorOr<LocationDto> locationResult = await _mapBoxClient.GetLocationFromZipCode(zipcode);
        return locationResult.Match(Ok, GetProblems);
    }

    /// <summary>
    /// Gets the location information from coordinates, such as the city, state, zipcode, and its country.
    /// </summary>
    /// <param name="longitude">Longitude coordinate value.</param>
    /// <param name="latitude">Latitude coordinate value.</param>
    /// <returns>Location object containing the zipcode and the location information.</returns>
    /// <response code="200">Returns the location object.</response>
    /// <response code="400">If the coordinates are invalid.</response>
    /// <response code="404">No location found with the given coordinates.</response>
    [HttpGet("Coordinates")]
    [ProducesResponseType(typeof(LocationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLocation([FromQuery] double longitude, [FromQuery] double latitude)
    {
        ErrorOr<LocationDto> locationResult = await _mapBoxClient.GetLocationFromCoordinates(longitude, latitude);
        return locationResult.Match(Ok, GetProblems);
    }
}