using System.ComponentModel.DataAnnotations;

namespace PetSearchAPI.RequestHelpers;

/// <summary>
/// Pet parameters to do a search query of a list of pets.
/// </summary>
public class PetsParams
{
    // Search for either dogs or cats.
    // Only support for cats and dogs for now...
    [Required]
    [RegularExpression(@"(?:dog|cat)$", ErrorMessage = "Only types: 'cat' and 'dog' are supported")]
    public required string Type { get; init; }

    // Zip code to look for pets. Only 5 digit zip codes for now...
    [Required]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be 5 digits.")]
    public required string Location { get; init; }

    // Request page number.
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
    public int Page { get; init; } = 1;

    // Get list of pets by distance from zipcode above
    [Range(0, 500, ErrorMessage = "Distance must be between 0-500")]
    public int Distance { get; init; } = 50;

    // recent, -recent, distance, -distance
    [RegularExpression(@"^-?(recent|distance)$",
        ErrorMessage = "Only location values: 'recent' and 'distance' with optional '-' prefix are accepted")]
    public string Sort { get; init; } = "distance";
}