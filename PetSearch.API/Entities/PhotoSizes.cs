using System.ComponentModel.DataAnnotations;

namespace PetSearch.API.Entities;

/// <summary>
/// The photo url sizes for a particular pet.
/// </summary>
public record PhotoSizes
{
    /// <summary>Small size url location:</summary>
    [Required]
    public required string Small { get; init; }

    /// <summary>Medium size url location.</summary>
    [Required]
    public required string Medium { get; init; }

    /// <summary>Large size url location.</summary>
    [Required]
    public required string Large { get; init; }

    /// <summary>Full size url location.</summary>
    [Required]
    public required string Full { get; init; }
};