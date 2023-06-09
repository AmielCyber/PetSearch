namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// The url photo sizes of a particular pet.
/// </summary>
public class PhotoSizesUrl
{
    public required string Small { get; init; }
    public required string Medium { get; init; }
    public required string Large { get; init; }
    public required string Full { get; init; }
}