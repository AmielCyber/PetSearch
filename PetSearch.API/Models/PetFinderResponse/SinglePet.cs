using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetSearch.API.Models.PetFinderResponse;

/// <summary>
/// Pet object containing the pet's attributes from a PetFinder API response.
/// </summary>
/// <param name="Pet">Pet details.</param>
public record SinglePet(
    [property: JsonPropertyName("animal"), Required]
    Entities.Pet Pet
);