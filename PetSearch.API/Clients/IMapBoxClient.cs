using Microsoft.AspNetCore.Http.HttpResults;
using PetSearch.API.Models;

namespace PetSearch.API.Clients;

/// <summary>
/// MapBox Client interface to handle MapBox API requests,
/// such as getting the location information from a zipcode or coordinates.
/// </summary>
public interface IMapBoxClient
{
    public Task<Results<Ok<LocationDto>, ProblemHttpResult>> GetLocationFromZipCode(string zipcode);
    public Task<Results<Ok<LocationDto>, ProblemHttpResult>> GetLocationFromCoordinates(double longitude, double latitude);
}