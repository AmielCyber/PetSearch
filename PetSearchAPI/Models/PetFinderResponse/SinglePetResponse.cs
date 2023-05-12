using System.Text.Json.Serialization;

namespace PetSearchAPI.Models.PetFinderResponse;

public class SinglePetResponse
{
    [JsonPropertyName("animal")]
    public PetDto Pet { get; set; }
}