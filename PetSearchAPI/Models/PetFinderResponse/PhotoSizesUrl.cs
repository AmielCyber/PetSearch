using System.ComponentModel.DataAnnotations;

namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// The photo url sizes for a particular pet.
/// </summary>
/// <param name="Small">Small size url location:</param>
/// <param name="Medium">Medium size url location.</param>
/// <param name="Large">Large size url location.</param>
/// <param name="Full">Full size url location.</param>
public record PhotoSizesUrl(
    [property: Required] string Small,
    [property: Required] string Medium,
    [property: Required] string Large,
    [property: Required] string Full
);