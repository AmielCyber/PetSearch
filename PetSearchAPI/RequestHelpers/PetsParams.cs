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
    public string Type { get; set; }

    // Zip code to look for pets. Only 5 digit zip codes for now...
    [Required]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be 5 digits.")]
    public string Location { get; set; }

    // Request page number.
    public int Page { get; set; } = 1;

    // Get list of pets by distance from zipcode above
    [Range(0, 500, ErrorMessage = "Distance must be between 0-500")]
    public int Distance { get; set; } = 50;

    // recent, -recent, distance, -distance
    [RegularExpression(@"^-?(recent|distance)$",
        ErrorMessage = "Only location values: 'recent' and 'distance' with optional '-' prefix are accepted")]
    public string Sort { get; set; } = "distance";
    
    public PetsParams(string type, string location)
    {
        Type = type;
        Location = location;
    }

}