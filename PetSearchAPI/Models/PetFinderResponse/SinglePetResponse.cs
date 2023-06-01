using System.Text.Json.Serialization;

namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// Pet object containing the pet's attributes.
/// </summary>
public class SinglePetResponse
{
    [JsonPropertyName("animal")] public PetDto Pet { get; set; }
}