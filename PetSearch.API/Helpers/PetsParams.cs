using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PetSearch.API.Helpers;

/// <summary>
/// Pet search query parameters.
/// </summary>
/// <param name="Type">Search for pet type, either 'dog' or 'cat'</param>
/// <param name="Location">Pets within given zip code. Only 5 digit zip codes are supported.</param>
/// <param name="Page">Page number in the pet list.</param>
/// <param name="Distance">Distance radius from location request zipcode.</param>
/// <param name="Sort">Sort pet list. Values(- descending): distance, -distance, recent, -recent</param>
public record PetsParams(
    [property: Required,
               RegularExpression(@"(?:dog|cat)$", ErrorMessage = "Only types: 'cat' and 'dog' are supported")]
    string Type,
    [property: Required,
               RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be 5 digits.")]
    string Location,
    [property: Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0."), DefaultValue(1)]
    int Page = 1,
    [property: Range(0, 500, ErrorMessage = "Distance must be between 0-500"), DefaultValue(25)]
    int Distance = 25,
    [property:
        RegularExpression(@"^-?(recent|distance)$",
            ErrorMessage = "Only location values: 'recent' and 'distance' with optional '-' prefix are accepted"),
        DefaultValue("distance")]
    string Sort = "distance"
);