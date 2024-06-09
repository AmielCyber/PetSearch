using ErrorOr;
using PetSearch.API.Models.MapBoxResponse;

namespace PetSearch.API.Clients;

/// <summary>
/// MapBox Client interface to handle MapBox API requests,
/// such as getting the location information from a zipcode or coordinates.
/// </summary>
public interface IMapBoxClient
{
    public Task<ErrorOr<LocationDto>> GetLocationFromZipCode(string zipcode);
    public Task<ErrorOr<LocationDto>> GetLocationFromCoordinates(double longitude, double latitude);
}