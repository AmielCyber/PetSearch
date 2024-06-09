using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PetSearch.API.Models.PetFinderResponse;

/// <summary>
/// Single Pet DTO containing attributes for a particular pet.
/// </summary>
public record PetDto
{
    /// <summary>Id number</summary>
    [Required]
    public int Id { get; init; }

    /// <summary>PetFinder URL for more information.</summary>
    [Required]
    public required string Url { get; init; }

    /// <summary>Type of pet: "Cat" | "Dog"</summary>
    [Required]
    public required string Type { get; init; }

    /// <summary>Age description: "Baby" | "Young" | "Adult" | "Senior"</summary>
    [Required]
    public required string Age { get; init; }

    /// <summary>Gender value: "Male" | "Female" | "Unknown"</summary>
    [Required]
    public required string Gender { get; init; }

    /// <summary>Size description: "small" | "medium" | "large" | "xlarge"</summary>
    [Required]
    public required string Size { get; init; }

    /// <summary>The name of the pet.</summary>
    [Required]
    public required string Name { get; init; }

    /// <summary>Optional pet description from PetFinder.</summary>
    public string? Description { get; init; }

    /// <summary>List of photos or an empty list if none are provided.</summary>
    public required PhotoSizesUrl[] Photos { get; init; } = Array.Empty<PhotoSizesUrl>();

    /// <summary>Optional cropped primary photo.</summary>
    [JsonPropertyName("primary_photo_cropped")]
    public PhotoSizesUrl? PrimaryPhotoSizesUrlCropped { get; init; }

    /// <summary>Status description(Most likely "Adoptable").</summary>
    [Required]
    public required string Status { get; init; }

    /// <summary>Distance from request location. Null if not from search but from GET PET/{id}.</summary>
    public double? Distance { get; init; }
}