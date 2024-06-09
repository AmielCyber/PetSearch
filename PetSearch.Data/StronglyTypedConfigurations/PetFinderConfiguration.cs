namespace PetSearch.Data.StronglyTypedConfigurations;

/// <summary>
/// Pet Finder Configuration to sent our credentials to use the PetSearch API.
/// </summary>
public class PetFinderConfiguration
{
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }
}