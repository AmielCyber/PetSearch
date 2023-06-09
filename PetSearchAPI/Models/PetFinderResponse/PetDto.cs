using System.Text.Json.Serialization;

namespace PetSearchAPI.Models.PetFinderResponse;

/// <summary>
/// Pet DTO describing all attributes of a pet to the client.
/// </summary>
public class PetDto
{
    public int Id { get; init; }
    public required string Url { get; init; }
    public required string Type { get; init; }
    public required string Age { get; init; }
    public required string Gender { get; init; }
    public required string Size { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required PhotoSizesUrl[] Photos { get; init; }

    [JsonPropertyName("primary_photo_cropped")]
    public PhotoSizesUrl? PrimaryPhotoSizesUrlCropped { get; init; }

    public required string Status { get; init; }
    public double? Distance { get; init; }
}