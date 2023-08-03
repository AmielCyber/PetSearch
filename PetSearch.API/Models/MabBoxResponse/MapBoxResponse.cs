using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetSearch.API.Models.MabBoxResponse;

/// <summary>
/// The response object we get from the MapBox API.
/// Only features object is needed for this application.
/// </summary>
public record MapBoxResponse
{
    /// <summary>
    /// The feature list which at most will contain 1 object due to our query to limit 1.
    /// </summary>
    [Required]
    [JsonPropertyName("features")]
    public List<MapBoxFeatures> FeaturesList { init; get; } = new List<MapBoxFeatures>();
}