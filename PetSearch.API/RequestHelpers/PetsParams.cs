using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PetSearch.API.RequestHelpers;

/// <summary>
/// Pets parameters to search for a pet.
/// </summary>
public record PetsParams
{
    /// <summary>Search for either 'dog' or 'cat'. Only support for cats and dogs for now...</summary>
    [Required]
    [RegularExpression(@"(?:dog|cat)$", ErrorMessage = "Only types: 'cat' and 'dog' are supported")]
    public required string Type { get; init; }

    /// <summary>Zip code to look for pets. Only 5 digit zip codes for now...</summary>
    [Required]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be 5 digits.")]
    public required string Location { get; init; }

    /// <summary>Page number (Greater than 1) in the search pet list.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
    [DefaultValue(1)]
    public int Page { get; init; } = 1;

    /// <summary>Get list of pets by distance range (0-500) from zipcode.</summary>
    [Range(0, 500, ErrorMessage = "Distance must be between 0-500")]
    [DefaultValue(50)]
    public int Distance { get; init; } = 50;

    /// <summary>Sort value: Default=distance, Other values: recent, -recent, -distance</summary>
    [DefaultValue("distance")]
    [RegularExpression(@"^-?(recent|distance)$",
        ErrorMessage = "Only location values: 'recent' and 'distance' with optional '-' prefix are accepted")]
    public string Sort { get; init; } = "distance";
};