using PetSearch.API.Entities;

namespace PetSearch.API.Tests.Data;

public static class PetInterfaceDefaultAttributes
{
    public const int Id = 123;
    public const string Url = "url";
    public const string Type = "type";
    public const string Age = "Age";
    public const string Gender = "Gender";
    public const string Size = "Size";
    public const string Name = "Name";
    public const string Description = "Description";
    public const string Status = "Status";
    public const int Distance = 25;
    
    public static PhotoSizes GetPhotoSizes()
    {
        return new PhotoSizes
        {
            Small = "Small",
            Medium = "Medium",
            Large = "Large",
            Full = "Full"
        };
    }

    public static List<PhotoSizes> GetPhotoSizesList()
    {
        var photos = new List<PhotoSizes>();
        for (int i = 0; i < 10; i++)
        {
            photos.Add(GetPhotoSizes());
        }

        return photos;
    }
}