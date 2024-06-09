namespace PetSearch.API.Models.MapBoxResponse;

/// <summary>
/// The location object to serve to our client application after a successful MapBox API request.
/// </summary>
/// <param name="Zipcode">A five digit zipcode.</param>
/// <param name="LocationName">The location name in the following format: {City}, {State} {Zipcode}, {Country}</param>
public record LocationDto(string Zipcode, string LocationName);