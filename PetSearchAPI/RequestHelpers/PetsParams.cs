using System.ComponentModel.DataAnnotations;

namespace PetSearchAPI.RequestHelpers;
public class PetsParams
{
    // Search for either dogs or cats.
    // Only support for cats and dogs for now...
    [Required]
    [RegularExpression(@"(?:dog|cat)$", ErrorMessage = "Only types: [cat , dog] are only supported")]
    public string Type { get; set; }
    
    // Zip code to look for pets. Only 5 digit zip codes for now...
    [Required]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be 5 digits.")]
    public string Location { get; set; }
    
    // Request page number.
    public int Page { get; set; } = 1;
    
    // Get list of pets by distance from zipcode above
    [Range(1,500, ErrorMessage = "Distance must be between 1-500")]
    public int Distance { get; set; } = 50;
    
    // recent, -recent, distance, -distance
    [RegularExpression(@"^-?(recent|distance)$", ErrorMessage = "Only distance and recent values accepted with optional '-' prefix")]
    public string Sort { get; set; } = "recent";    
}