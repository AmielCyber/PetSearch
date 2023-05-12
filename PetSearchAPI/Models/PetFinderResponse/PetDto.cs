using System.Text.Json.Serialization;

namespace PetSearchAPI.Models.PetFinderResponse;

public class PetDto
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string Type { get; set; }
    public string Age { get; set; }
    public string Gender { get; set; }
    public string Size { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public PhotoSizesUrl[] Photos { get; set; }
    [JsonPropertyName("primary_photo_cropped")]
    public PhotoSizesUrl PrimaryPhotoSizesUrlCropped { get; set; }
    public string Status { get; set; }
    public double? Distance { get; set; }
}