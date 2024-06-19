using System.ComponentModel.DataAnnotations;
using PetSearch.API.Entities;

namespace PetSearch.API.Models;

/// <summary>
/// Pet DTO for client containing attributes for a particular pet.
/// </summary>
public record PetDto : IPet
{
    /// <summary>
    /// Id number
    /// </summary>
    [Required]
    public int Id { get; init; }

    /// <summary>
    /// PetFinder URL for the user to see more information than this application such as contact details.
    /// </summary>
    [Required]
    public required string Url { get; init; }

    /// <summary>
    /// Type of pet: "Cat" or "Dog".
    /// </summary>
    [Required]
    public required string Type { get; init; }

    /// <summary>
    /// Age description: "Baby", "Young", "Adult", or "Senior".
    /// </summary>
    [Required]
    public required string Age { get; init; }

    /// <summary>
    /// Gender value: "Male", "Female", or "Unknown".
    /// </summary>
    [Required]
    public required string Gender { get; init; }

    /// <summary>
    /// Size description: "small", "medium", "large", or "xlarge"
    /// </summary>
    [Required]
    public required string Size { get; init; }

    /// <summary>
    /// The name of the pet.
    /// </summary>
    [Required]
    public required string Name { get; init; }

    /// <summary>
    /// Optional pet description from the author.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// List of url photos with different photo sizes for the pet, or an empty list if none are provided.
    /// </summary>
    public required IEnumerable<PhotoSizes> Photos { get; init; } = [];

    /// <summary>
    /// A primary cropped photo with four different sizes: small, medium, large, or full 
    /// </summary>
    public PhotoSizes? PrimaryPhotoCropped { get; init; }

    /// <summary>
    /// The current adoption status. Most likely to be "Adoptable" when fetch from route "api/pets".
    /// </summary>
    [Required]
    public required string Status { get; init; }

    /// <summary>
    /// Distance from the requested location. Null if requested from the route "api/pets/{petId}".
    /// </summary>
    public double? Distance { get; init; }
}