using System.Text.Json.Serialization;

namespace PetSearch.API.Entities;

/// <summary>
/// Pet entity that we get from the PetFinder API.
/// </summary>
public record Pet : IPet
{
    /// <summary>
    /// Id number
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// PetFinder URL for this pet
    /// </summary>
    public required string Url { get; init; }

    /// <summary>
    /// Pet type: "Cat" | "Dog"
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Age description: "Baby" | "Young" | "Adult" | "Senior"
    /// </summary>
    public required string Age { get; init; }

    /// <summary>
    /// Gender value: "Male" | "Female" | "Unknown"
    /// </summary>
    public required string Gender { get; init; }

    /// <summary>
    /// Size description: "small" | "medium" | "large" | "xlarge"
    /// </summary>
    public required string Size { get; init; }

    /// <summary>
    /// The name of the pet.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Optional pet description from the author.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// List of url photos with different photo sizes for the pet, or an empty list if none are provided.
    /// </summary>
    public required IEnumerable<PhotoSizes> Photos { get; init; } = [];

    [JsonPropertyName("primary_photo_cropped")]
    public PhotoSizes? PrimaryPhotoCropped { get; init; }

    /// <summary>
    /// The current adoption status: "adoptable" | "adopted" | "found", or others
    /// </summary>
    public required string Status { get; init; }

    /// <summary>
    /// Distance from the requested location. Null if requested a single pet.
    /// </summary>
    public double? Distance { get; init; }
};