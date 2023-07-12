using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// Pet object containing the pet's attributes.
/// </summary>
/// <param name="Pet">Pet details.</param>
public record SinglePetResponse(
    [property: JsonPropertyName("animal"), Required]
    PetDto Pet
);