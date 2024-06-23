namespace PetSearch.Data.StronglyTypedConfigurations;

/// <summary>
/// Pet Finder Configuration to sent our credentials to use the PetSearch API.
/// </summary>
public class PetFinderConfiguration
{
    /// <summary>
    /// Uri to make PetFinder requests
    /// </summary>
    public const string Uri = "https://api.petfinder.com/v2/";

    /// <summary>
    /// Client id provided by PetFinder API
    /// </summary>
    public string? ClientId { get; init; }

    /// <summary>
    /// Client secret provided by PetFinder API
    /// </summary>
    public string? ClientSecret { get; init; }
}