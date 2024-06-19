namespace PetSearch.API.Entities;

/// <summary>
/// Pet interface that share common properties from our entity class and our
/// DTO that will be exposed to the client.
/// </summary>
public interface IPet
{
    public int Id { get; init; }
    public string Url { get; init; }
    public string Type { get; init; }
    public string Age { get; init; }
    public string Gender { get; init; }
    public string Size { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public IEnumerable<PhotoSizes> Photos { get; init; }
    public double? Distance { get; init; }
}