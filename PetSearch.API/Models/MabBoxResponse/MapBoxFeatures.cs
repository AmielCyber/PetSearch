using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetSearch.API.Models.MabBoxResponse;

/// <summary>
/// The MapBox response features that we will only use for this application.
/// </summary>
/// <param name="Zipcode">A five digit zipcode.</param>
/// <param name="LocationName">The location name in the following format: City, State Zipcode, Country</param>
public record MapBoxFeatures(
    [property: Required, JsonPropertyName("text_en")]
    string Zipcode,
    [property: Required, JsonPropertyName("place_name_en")]
    string LocationName);