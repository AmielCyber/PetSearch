using System.ComponentModel.DataAnnotations;

namespace PetSearchAPI.RequestHelpers;

/// <summary>
/// Pets parameters to search for a pet.
/// </summary>
/// <param name="Type">Search for either 'dogs' or 'cats'. Only support for cats and dogs for now...</param>
/// <param name="Location">Zip code to look for pets. Only 5 digit zip codes for now...</param>
/// <param name="Page">Page number (Greater than 1) in the search pet list.</param>
/// <param name="Distance">Get list of pets by distance range (0-500) from zipcode.</param>
/// <param name="Sort">Sort value: Default=distance, Other values: recent, -recent, -distance</param>
public record PetsParams
(
    [Required]
    [RegularExpression(@"(?:dog|cat)$", ErrorMessage = "Only types: 'cat' and 'dog' are supported")]
    string Type,
    [Required]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be 5 digits.")]
    string Location,
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
    int Page = 1,
    [Range(0, 500, ErrorMessage = "Distance must be between 0-500")]
    int Distance = 50,
    [RegularExpression(@"^-?(recent|distance)$",
        ErrorMessage = "Only location values: 'recent' and 'distance' with optional '-' prefix are accepted")]
    string Sort = "distance"
);