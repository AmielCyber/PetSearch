namespace PetSearch.API.Entities;

/// <summary>
/// The photo url sizes for a particular pet.
/// </summary>
public record PhotoSizes
{
    /// <summary>Small size url location:</summary>
    public required string Small { get; init; }

    /// <summary>Medium size url location.</summary>
    public required string Medium { get; init; }

    /// <summary>Large size url location.</summary>
    public required string Large { get; init; }

    /// <summary>Full size url location.</summary>
    public required string Full { get; init; }
};